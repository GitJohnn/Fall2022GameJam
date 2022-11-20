using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
   [SerializeField] bool _useCustomAttackSettings = false; 
   [SerializeField] AttackSettings _settings;
   
   [SerializeField, ShowIf("_useCustomAttackSettings")] int _meleeDamage;
   [SerializeField, ShowIf("_useCustomAttackSettings")] float _timeBetweenAttacks;
   [SerializeField, ShowIf("_useCustomAttackSettings")] LayerMask _includeLayers;
   [SerializeField, ShowIf("_useCustomAttackSettings")] float _attackRange;
   [SerializeField] UnityEvent _onPrimaryAttack;
   [SerializeField] UnityEvent _onSecondaryAttack;
   [SerializeField] Transform _attackPosition;

   int MeleeDamage 
   {
      get 
      {
         if (_useCustomAttackSettings) return _meleeDamage;
         else return _settings.MeleeDamage;
      }
   }

   float TimeBetweenAttacks 
   {
      get 
      {
         if (_useCustomAttackSettings) return _timeBetweenAttacks;
         else return _settings.AttackDelay;
      }
   }

   LayerMask IncludeLayers 
   {
      get 
      {
         if (_useCustomAttackSettings) return _includeLayers;
         else return _settings.IncludeLayers;
      }
   }

   float AttackRange 
   {
      get 
      {
         if (_useCustomAttackSettings) return _attackRange;
         else return _settings.AttackRange;
      }
   }

   [SerializeField, ReadOnly] float _timeBetweenAttack;

   void Update()
   {
      if (_timeBetweenAttack <= 0)
      {
         if (Input.GetMouseButtonDown(0))
         {
            // Debug.Log("REE");
            HandlePrimaryAttack();
            _timeBetweenAttack = TimeBetweenAttacks;

         }
         else if (Input.GetMouseButtonDown(1))
         {
            HandleSecondaryAttack();
         }
      }
      else
      {
         _timeBetweenAttack -= Time.deltaTime;
      }

   }

   void HandlePrimaryAttack()
   {
      _onPrimaryAttack?.Invoke();
      Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPosition.position, AttackRange, IncludeLayers);
      foreach (Collider2D collider in targets)
      {
         // Debug.Log(collider.name); 
         HandleHitLogic(collider);
      }
   }

   void HandleSecondaryAttack()
   {
      _onSecondaryAttack?.Invoke();
   }
   void HandleHitLogic(Collider2D collider)
   {
      var targetHealth = collider.GetComponent<Health>();
      if (!targetHealth) return;

      targetHealth.GetHit(MeleeDamage, gameObject);
   }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPosition.position, AttackRange);
    }
}
