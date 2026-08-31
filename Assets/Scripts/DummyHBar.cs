using UnityEngine;
using UnityEngine.UI;

public class DummyHBar : MonoBehaviour
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
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Prevent health from going below 0
        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        // Update health bar
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. Remaining health: " +
            currentHealth
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(
            gameObject.name +
            " was destroyed."
        );

        Destroy(gameObject);
    }
}