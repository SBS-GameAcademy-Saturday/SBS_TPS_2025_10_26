using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;

    public UnityEvent<float, float> OnHealthChangedEvent;
    public UnityEvent OnDeath;

    public float CurrentHealth
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    void Awake()
    {
        health = maxHealth;
    }

    public void HitDamage(float amount)
    {
        health -= amount;
        OnHealthChangedEvent.Invoke(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
    }
}
