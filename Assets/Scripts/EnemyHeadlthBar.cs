using UnityEngine;
using UnityEngine.UI;

public class EnemyHeadlthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Damagable damagable;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        damagable.OnHealthChangedEvent.AddListener(OnHealthChanaged);
    }

    private void Update()
    {
        transform.LookAt(player);
    }

    private void OnHealthChanaged(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
    }


}
