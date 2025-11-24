using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100f;
    private float _currentHealth;

    public void Init(float maxHealth)
    {
        MaxHealth = maxHealth;
        _currentHealth = MaxHealth;
    }
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
