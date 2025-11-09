using UnityEngine;
using UnityEngine.UI;

public class EnemyHeadlthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Damagable damagable;

    private void Start()
    {
        damagable.OnHealthChangedEvent.AddListener(OnHealthChanaged);
    }

    private void OnHealthChanaged(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }
}
