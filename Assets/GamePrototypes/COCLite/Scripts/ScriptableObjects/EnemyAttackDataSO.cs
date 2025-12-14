using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "TopDownShooter/EnemyAttackData", order = 2)]
public class EnemyAttackDataSO : ScriptableObject
{
    public AttackType attackType = AttackType.Melee;
    public float AttackDamage = 10f;
    public float AttackCooldown = 2f;

    [Header("Ranged attacks")]
    public float RangedAttackRange = 5f;
    public GameObject ProjectilePrefab;
}

public enum AttackType
{
    Melee,
    Ranged
}
