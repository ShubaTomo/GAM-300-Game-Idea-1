using UnityEngine;
using UnityEngine.UI;

public class HelmetDamageHUD : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private Image damageImage;

    private void Start()
    {
        damageImage = GetComponent<Image>();
    }

    private void Update()
    {
        float healthPercent =
            playerHealth.currentHealth /
            playerHealth.maxHealth;

        Color color = damageImage.color;

        // 100 HP = alpha 0
        // 0 HP = alpha 1
        color.a = 1f - healthPercent;

        damageImage.color = color;
    }
}