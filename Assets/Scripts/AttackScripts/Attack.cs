using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoundSystem;

public enum UserType
{
    Player,
    Enemy
}

public class Attack : MonoBehaviour
{
	[SerializeField] private bool _useDefaultAttackSettings = false;
	[SerializeField, ShowIf("_useDefaultAttackSettings")] protected AttackSettings _settings;

	[SerializeField] bool _useCustomAttackSettings = false;
	[SerializeField, ShowIf("_useCustomAttackSettings")] float _customMeleeDamage;
	[SerializeField, ShowIf("_useCustomAttackSettings")] float _customTimeBetweenAttacks;
	[SerializeField, ShowIf("_useCustomAttackSettings")] LayerMask _customIncludeLayers;
	[SerializeField, ShowIf("_useCustomAttackSettings")] float _customAttackRange;
   
	[SerializeField] bool _usePlayerStats = false;
	[SerializeField, ShowIf("_usePlayerStats")] private PlayerStats _playerStats;
	[SerializeField, ShowIf("_usePlayerStats")] LayerMask _playerIncludeLayers;
	[SerializeField, ShowIf("_usePlayerStats")] float _playerAttackRange;

	[SerializeField] protected UnityEvent _onPrimaryAttack;
	//[SerializeField] protected UnityEvent _onSecondaryAttack;
	[SerializeField] protected Transform _attackPosition;
	[SerializeField] SFXEvent _sfxStaffSwing;
    
	[SerializeField] private bool _canAttack;

    private Rigidbody2D _rigidbody2D;

	float MeleeDamage 
	{
		get 
		{
			if(_useCustomAttackSettings) return _customMeleeDamage;
			else if (_usePlayerStats) return _playerStats.AttackPower;
			else return _settings.MeleeDamage;
		}
	}

	float TimeBetweenAttacks 
	{
		get 
		{
			if(_useCustomAttackSettings) return _customTimeBetweenAttacks;
			else if(_usePlayerStats) return _playerStats.AttackDelay;
			else return _settings.AttackDelay;
		}
	}

   LayerMask IncludeLayers 
   {
      get 
      {
         if (_useCustomAttackSettings) return _customIncludeLayers;
         else return _settings.IncludeLayers;
      }
   }

   float AttackRange 
   {
      get 
      {
         if (_useCustomAttackSettings) return _customAttackRange;
         else return _settings.AttackRange;
      }
   }

   [SerializeField, ReadOnly] float _timeSinceLastAttack;

    private void Awake()
	{
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		_timeSinceLastAttack += Time.deltaTime;
    }    

    public virtual void HandlePrimaryAttack()
    {
	    if (!_canAttack) return;
		if(_timeSinceLastAttack < TimeBetweenAttacks) return;
        _sfxStaffSwing.Play();
        _onPrimaryAttack?.Invoke();
        Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPosition.position, AttackRange, IncludeLayers);
        foreach (Collider2D collider in targets)
        {
            // Debug.Log(collider.name); 
            HandleHitLogic(collider);
        }
		_timeSinceLastAttack = 0;
    }
    
	public void CanAttack()
	{
		_canAttack = true;
	}

    //Dash mechanic
    //public virtual void HandleSecondaryAttack()
    //{
    //    _onSecondaryAttack?.Invoke();
    //}

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
