using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GlassCrackController glassCrackController;

    public float maxHealth = 100f;
    public float currentHealth;

    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (glassCrackController != null)
        {
            glassCrackController.onTakeDamage(damage);
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0,
            maxHealth);

        Debug.Log(
            "Player HP: " +
            currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log("Player Died");

        // Add death screen later
    }
}