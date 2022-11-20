using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat { MaxHealth, AttackPower, Defense, Speed, AttackSpeed }

[CreateAssetMenu (menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject {

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

    private float maxHealthPercentageChange = 0;
    private float attackPowerPercentageChange = 0;
    private float defensePercentageChange = 0;
    private float speedPercentageChange = 0;
    private float attackSpeedPercentageChange = 0;

    public float MaxHealth => maxHealth;
    public float AttackPower => attackPower;
    public float Defense => defense;
    public float Speed => speed;
    public float AttackSpeed => attackSpeed;

    //call at the beginning of a dungeon/scene to reset the stats to base levels
    public void InitializeStats() {
        maxHealth = maxHealthBase;
        attackPower = attackPowerBase;
        defense = defenseBase;
        speed = speedBase;
        attackSpeed = attackSpeedBase;
    }

    public void ChangeMaxHealth(float percentageChange) {
        maxHealthPercentageChange += percentageChange;
        maxHealth = maxHealthBase + (maxHealthBase * (maxHealthPercentageChange / 10));
    }

    public void ChangeAttackPower(float percentageChange) {
        attackPowerPercentageChange += percentageChange;
        attackPower = attackPowerBase + (attackPowerBase * (attackPowerPercentageChange / 10));
    }

    public void ChangeDefense(float percentageChange) {
        defensePercentageChange += percentageChange;
        defense = defenseBase + (defenseBase * (defensePercentageChange / 10));
    }

    public void ChangeSpeed(float percentageChange) {
        speedPercentageChange += percentageChange;
        speed = speedBase + (speedBase * (speedPercentageChange / 10));
    }

    public void ChangeAttackSpeed(float percentageChange) {
        attackSpeedPercentageChange += percentageChange;
        attackSpeed = attackSpeedBase + (attackSpeedBase * (attackSpeedPercentageChange / 10));
    }
}