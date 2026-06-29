using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static bool isDead;
    public PlayerHealth playerHealth;

    public GameObject deathCanvas;

    private void Start()
    {
        isDead = false;
        deathCanvas.SetActive(false);
    }

    private void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            isDead = true;
            deathCanvas.SetActive(true);
        }
    }
}
