using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UserType
{
    Player,
    Enemy
}

public class Attack : MonoBehaviour
{
   [SerializeField] bool _useCustomAttackSettings = false; 
   [SerializeField] protected AttackSettings _settings;
   
   [SerializeField, ShowIf("_useCustomAttackSettings")] int _meleeDamage;
   [SerializeField, ShowIf("_useCustomAttackSettings")] float _timeBetweenAttacks;
   [SerializeField, ShowIf("_useCustomAttackSettings")] LayerMask _includeLayers;
   [SerializeField, ShowIf("_useCustomAttackSettings")] float _attackRange;
   [SerializeField] protected UnityEvent _onPrimaryAttack;
   [SerializeField] protected UnityEvent _onSecondaryAttack;
   [SerializeField] protected Transform _attackPosition;

    private Rigidbody2D _rigidbody2D;

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

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_timeBetweenAttack <= 0)
        {
            if (_settings.UserType == UserType.Player) // If the user is a player
            {
                //if (Input.GetMouseButtonDown(0))// and if the player presses the left mouse button
                //{
                //    // Debug.Log("REE");
                //    HandlePrimaryAttack(); // it will do the attack
                //    _timeBetweenAttack = TimeBetweenAttacks; // and reset the clock

                //}
                //else if (Input.GetMouseButtonDown(1)) // if the player presses the left mouse button
                //{
                //    HandleSecondaryAttack(); // it'll do the secondary attack
                //}
            }
            else // But if the user isn't a player
            {
                // HandlePrimaryAttack(); // it'll only run the primary attack logic
                // _timeBetweenAttack = TimeBetweenAttacks; // and then reset the clock
            }
        }
        else
        {
            _timeBetweenAttack -= Time.deltaTime;
        }
    }    

    public virtual void HandlePrimaryAttack()
    {
        _onPrimaryAttack?.Invoke();
        Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPosition.position, AttackRange, IncludeLayers);
        foreach (Collider2D collider in targets)
        {
            // Debug.Log(collider.name); 
            HandleHitLogic(collider);
        }
    }

    //Dash mechanic
    public virtual void HandleSecondaryAttack()
    {
        _onSecondaryAttack?.Invoke();
    }

    public virtual void HandleHitLogic(Collider2D collider)
    {
        var targetHealth = collider.GetComponent<HealthBase>();
        if (!targetHealth) return;

        targetHealth.GetHit(MeleeDamage, gameObject);
    }

    void OnDrawGizmos()
    {
        if (!_attackPosition)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPosition.position, AttackRange);
    }
}
