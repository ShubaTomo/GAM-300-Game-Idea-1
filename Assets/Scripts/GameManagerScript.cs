using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static bool isDead = false;
    public PlayerHealth playerHealth;

    private void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            isDead = true;
        }
    }
}
