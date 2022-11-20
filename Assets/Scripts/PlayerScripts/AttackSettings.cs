using UnityEngine;

[CreateAssetMenu(menuName = "Player/AttackSettings", fileName = "New Player Attack Settings")]
public class AttackSettings : ScriptableObject
{
    [Tooltip("Amount of damage on a single hit")]
    [SerializeField] int _meleeDamage;

    [SerializeField] float _startTimeBetweenAttack;
    [SerializeField] LayerMask _includeLayers;
    [SerializeField] float _attackRange;
    

    public int MeleeDamage => _meleeDamage;
    public float StartTimeBetweenAttack => _startTimeBetweenAttack;
    public LayerMask IncludeLayers => _includeLayers;
    public float AttackRange => _attackRange;

}