using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat { MaxHealth, AttackPower, Defense, Speed, AttackSpeed }

public class PlayerStats : MonoBehaviour {

    [SerializeField] private float maxHealthBase;
    [SerializeField, ReadOnly] private float maxHealth;
    [SerializeField] private float attackPowerBase;
    [SerializeField, ReadOnly] private float attackPower;
    [SerializeField] private float defenseBase;
    [SerializeField, ReadOnly] private float defense;
    [SerializeField] private float speedBase;
    [SerializeField, ReadOnly] private float speed;
    [SerializeField] private float attackSpeedBase;
    [SerializeField, ReadOnly] private float attackSpeed;
    [SerializeField] private float toxicityThreshold;
    [SerializeField, ReadOnly] private float toxicityLevel;

    private float maxHealthPercentageChange = 0;
    private float attackPowerPercentageChange = 0;
    private float defensePercentageChange = 0;
    private float speedPercentageChange = 0;
    private float attackSpeedPercentageChange = 0;

    private float minimumStatVal = 1;

    public float MaxHealth => maxHealth;
    public float AttackPower => attackPower;
    public float Defense => defense;
    public float Speed => speed;
    public float AttackSpeed => attackSpeed;
    public float ToxicityLevel => toxicityLevel;

    public event Action<float> MaxHealthChanged;
    public event Action<float> AttackPowerChanged;
    public event Action<float> DefenseChanged;
    public event Action<float> SpeedChanged;
    public event Action<float> AttackSpeedChanged;

    private void Awake() {
        InitializeStats();
    }

    //call at the beginning of a dungeon/scene to reset the stats to base levels
    public void InitializeStats() {
        maxHealth = maxHealthBase;
        attackPower = attackPowerBase;
        defense = defenseBase;
        speed = speedBase;
        attackSpeed = attackSpeedBase;
    }

    public void ChangeMaxHealth(float percentageChange) {
        maxHealth = StatChangeHelper(maxHealthBase, maxHealthPercentageChange, percentageChange);
        MaxHealthChanged?.Invoke(maxHealth);
    }

    public void ChangeAttackPower(float percentageChange) {
        attackPower = StatChangeHelper(attackPowerBase, attackPowerPercentageChange, percentageChange);
        AttackPowerChanged?.Invoke(attackPower);
    }

    public void ChangeDefense(float percentageChange) {
        defense = StatChangeHelper(defenseBase, defensePercentageChange, percentageChange);
        DefenseChanged?.Invoke(defense);
    }

    public void ChangeSpeed(float percentageChange) {
        speed = StatChangeHelper(speedBase, speedPercentageChange, percentageChange);
        SpeedChanged?.Invoke(speed);
    }

    public void ChangeAttackSpeed(float percentageChange) {
        attackSpeed = StatChangeHelper(attackSpeedBase, attackSpeedPercentageChange, percentageChange);
        AttackSpeedChanged?.Invoke(attackSpeed);
    }

    private float StatChangeHelper(float baseStat, float statPercentageChange, float newPercentageChange) {
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
    }
}