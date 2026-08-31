using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public SpeedDamage speedDamage;

    public TMP_Text speedText;
    public TMP_Text damageTierText;
    public TMP_Text damageText;

    [Header("Display")]
    public float displayMultiplier = 1f;
    public int decimalPlaces = 1;

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();

        if (speedDamage == null)
            speedDamage = FindObjectOfType<SpeedDamage>();
    }

    private void Update()
    {
        if (playerMovement == null)
            return;

        // Get current player speed
        float speed = playerMovement.GetSpeed() * displayMultiplier;

        string speedFormat = "F" + decimalPlaces;

        // Speed display
        // Example: SPEED: 27.4 m/s
        speedText.text =
            "SPEED: " +
            speed.ToString(speedFormat) +
            " m/s";

        // Damage information
        if (speedDamage != null)
        {
            // Example: MEDIUM
            damageTierText.text =
                speedDamage.GetDamageTier();

            // Example: 25 DAMAGE
            damageText.text =
                speedDamage.GetCurrentDamage()
                .ToString("F0") +
                " DAMAGE";
        }
    }
}