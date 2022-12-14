using UnityEngine;

[CreateAssetMenu(menuName = "Player/AttackSettings", fileName = "New Player Attack Settings")]
public class AttackSettings : ScriptableObject
{
    [Tooltip("Amount of damage on a single hit")]
    [SerializeField] int _meleeDamage;
    [SerializeField] UserType _userType;

    [SerializeField] float _attackDelay;
    [SerializeField] LayerMask _includeLayers;
    [SerializeField] float _attackRange;
    

    public UserType UserType => _userType;
    public int MeleeDamage => _meleeDamage;
    public float AttackDelay => _attackDelay;
    public LayerMask IncludeLayers => _includeLayers;
    public float AttackRange => _attackRange;

}