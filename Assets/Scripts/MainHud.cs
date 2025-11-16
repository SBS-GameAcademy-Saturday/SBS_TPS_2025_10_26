using UnityEngine;
using UnityEngine.UI;

public class MainHud : MonoBehaviour
{
    [SerializeField] private Damagable playerDamagable;

    [SerializeField] private Slider playerHealthBar;

    [SerializeField] private Image crossHair;

    void Start()
    {
        playerDamagable.OnHealthChangedEvent.AddListener(OnHealthChanged);
        OnHealthChanged(playerDamagable.CurrentHealth, playerDamagable.MaxHealth);
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        playerHealthBar.value = currentHealth / maxHealth;
    }

    public void CanCollectiveState(bool state)
    {
        crossHair.color = state ? Color.blue : Color.white;
    }
}
