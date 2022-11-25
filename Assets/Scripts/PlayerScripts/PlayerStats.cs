using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat { MaxHealth, AttackPower, Defense, Speed, AttackDelay }

public class PlayerStats : MonoBehaviour {

    [SerializeField] private float maxHealthBase;
    [SerializeField, ReadOnly] private float maxHealth;
    [SerializeField] private float attackPowerBase;
    [SerializeField, ReadOnly] private float attackPower;
    [SerializeField] private float defenseBase;
    [SerializeField, ReadOnly] private float defense;
    [SerializeField] private float speedBase;
    [SerializeField, ReadOnly] private float speed;
    [SerializeField] private float attackDelayBase;
    [SerializeField, ReadOnly] private float attackDelay;
    [SerializeField] private float toxicityThreshold;
    [SerializeField, ReadOnly] private float toxicityLevel;

    private float maxHealthPercentageChange = 0;
    private float attackPowerPercentageChange = 0;
    private float defensePercentageChange = 0;
    private float speedPercentageChange = 0;
    private float attackDelayPercentageChange = 0;

    private float minimumStatVal = 0.1f;

    public float MaxHealth => maxHealth;
    public float AttackPower => attackPower;
    public float Defense => defense;
    public float Speed => speed;
    public float AttackDelay => attackDelay;
    public float ToxicityThreshold => toxicityThreshold;
    public float ToxicityLevel => toxicityLevel;

    public event Action<float> MaxHealthChanged;
    public event Action<float> AttackPowerChanged;
    public event Action<float> DefenseChanged;
    public event Action<float> SpeedChanged;
    public event Action<float> AttackDelayChanged;
    public event Action<float> ToxicityChanged;

    private void Awake() {
        InitializeStats();
    }

    //call at the beginning of a dungeon/scene to reset the stats to base levels
    public void InitializeStats() {
        maxHealth = maxHealthBase;
        attackPower = attackPowerBase;
        defense = defenseBase;
        speed = speedBase;
        attackDelay = attackDelayBase;
    }

    public void ChangeMaxHealth(float percentageChange) {
        maxHealth = StatChangeHelper(maxHealthBase, ref maxHealthPercentageChange, percentageChange);
        MaxHealthChanged?.Invoke(maxHealth);
    }

    public void ChangeAttackPower(float percentageChange) {
        attackPower = StatChangeHelper(attackPowerBase, ref attackPowerPercentageChange, percentageChange);
        AttackPowerChanged?.Invoke(attackPower);
    }

    public void ChangeDefense(float percentageChange) {
        defense = StatChangeHelper(defenseBase, ref defensePercentageChange, percentageChange);
        DefenseChanged?.Invoke(defense);
    }

    public void ChangeSpeed(float percentageChange) {
        speed = StatChangeHelper(speedBase, ref speedPercentageChange, percentageChange);
        SpeedChanged?.Invoke(speed);
    }

    public void ChangeAttackDelay(float percentageChange) {
        attackDelay = StatChangeHelper(attackDelayBase, ref attackDelayPercentageChange, -percentageChange);
        AttackDelayChanged?.Invoke(attackDelay);
    }

    private float StatChangeHelper(float baseStat, ref float statPercentageChange, float newPercentageChange) {
        statPercentageChange += newPercentageChange;
        float newStat = baseStat + (baseStat * (statPercentageChange / 100));
        if(newStat < minimumStatVal) newStat = minimumStatVal;
        return newStat;
    }

    public void ChangeToxicity(float newToxicity) {
        toxicityLevel += newToxicity;
        if(toxicityLevel >= toxicityThreshold) {
            Debug.Log("toxicity threshold passed");
            //TODO: put something else here
        }
        ToxicityChanged?.Invoke(toxicityLevel);
    }
}