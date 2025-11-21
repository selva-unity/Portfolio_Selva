using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackBehaviour
{
    private Enemy _owner;
    private EnemyAttackDataSO _attackData;
    private float _cooldownTimer;
    private Transform _target;

    public void Initialize(Enemy owner, EnemyAttackDataSO attackdata)
    {
        _owner = owner;
        _attackData = attackdata;
        _cooldownTimer = 0f;
    }

    public void Attack()
    {
        if(_target == null) return;
        Debug.Log("MeleeAttack Attack called");
    }
}
