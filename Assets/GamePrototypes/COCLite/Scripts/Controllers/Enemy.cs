using UnityEngine;
using COCLite.Controllers;
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStats;
    [SerializeField] private EnemyAttackDataSO enemyAttackData;
    private IAttackBehaviour _attackBehaviour;
    private Health _health;

    public EnemyStatsSO EnemyStats => enemyStats;
    public EnemyAttackDataSO EnemyAttackData => enemyAttackData;

    void Awake()
    {
        _health = GetComponent<Health>();
        _health.Init(enemyStats.MaxHealth);

        _attackBehaviour = GetComponent<IAttackBehaviour>();
        if (_attackBehaviour != null)
        {
            _attackBehaviour.Initialize(this, enemyAttackData);
        }
        else
        {
            Debug.LogError("No IAttackBehaviour found on Enemy.");
        }
    }

    void Update()
    {
        _attackBehaviour?.Attack();
    }


}

public interface IAttackBehaviour
{
    void Initialize(Enemy owner, EnemyAttackDataSO data);
    void Attack();
}
