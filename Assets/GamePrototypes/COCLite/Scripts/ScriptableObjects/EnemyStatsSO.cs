using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "TopDownShooter/EnemyStats", order = 1)]
public class EnemyStatsSO : ScriptableObject
{
    public string enemyName = "Enemy";
    public float MaxHealth = 100f;
    public float MoveSpeed = 3.5f;
}
