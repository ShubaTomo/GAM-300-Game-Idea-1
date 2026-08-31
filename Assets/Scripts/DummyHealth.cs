using UnityEngine;
using UnityEngine.UI;

public class DummyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;

    private float currentHealth;

    [Header("Health Bar")]
    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.minValue = 0f;
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("Health Bar is NOT assigned!");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. Remaining health: " +
            currentHealth
        );

        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;

            Debug.Log(
                "Health Bar Updated: " +
                healthBar.value
            );
        }
        else
        {
            Debug.LogWarning(
                "Health Bar is NULL on " +
                gameObject.name
            );
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " was destroyed.");

        Destroy(gameObject);
    }
}