using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    [SerializeField] private Damagable playerDamagable;

    [SerializeField] private Slider playerHealthBar;

    void Start()
    {
        playerDamagable.OnHealthChangedEvent.AddListener(OnHealthChanged);
        OnHealthChanged(playerDamagable.CurrentHealth, playerDamagable.MaxHealth);
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        playerHealthBar.value = currentHealth / maxHealth;
    }
}
