using UnityEngine;

public class SpeedDamage : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;

    [Header("Speed Thresholds")]
    public float lightSpeed = 10f;
    public float mediumSpeed = 20f;
    public float heavySpeed = 30f;

    [Header("Damage")]
    public float lightDamage = 10f;
    public float mediumDamage = 25f;
    public float heavyDamage = 50f;

    [Header("Impact")]
    public float minimumImpactSpeed = 10f;

    private float currentSpeed;
    private float currentDamage;
    private string currentDamageTier = "NONE";

    private void Start()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement == null)
            return;

        // Get current player speed
        currentSpeed = playerMovement.GetSpeed();

        // Determine damage based on speed
        UpdateDamageTier();
    }

    private void UpdateDamageTier()
    {
        if (currentSpeed >= heavySpeed)
        {
            currentDamage = heavyDamage;
            currentDamageTier = "HEAVY";
        }
        else if (currentSpeed >= mediumSpeed)
        {
            currentDamage = mediumDamage;
            currentDamageTier = "MEDIUM";
        }
        else if (currentSpeed >= lightSpeed)
        {
            currentDamage = lightDamage;
            currentDamageTier = "LIGHT";
        }
        else
        {
            currentDamage = 0f;
            currentDamageTier = "NONE";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("PLAYER COLLIDED WITH: " + collision.gameObject.name);

        DummyHealth target =
            collision.gameObject.GetComponentInParent<DummyHealth>();

        if (target == null)
        {
            Debug.Log("NO TARGET HEALTH FOUND");
            return;
        }

        float impactSpeed = currentSpeed;

        Debug.Log("IMPACT SPEED: " + impactSpeed);

        // Don't deal damage if moving too slowly
        if (impactSpeed < minimumImpactSpeed)
        {
            Debug.Log("IMPACT TOO SLOW - NO DAMAGE");
            return;
        }

        // Update damage based on the speed at impact
        if (impactSpeed >= heavySpeed)
        {
            currentDamage = heavyDamage;
            currentDamageTier = "HEAVY";
        }
        else if (impactSpeed >= mediumSpeed)
        {
            currentDamage = mediumDamage;
            currentDamageTier = "MEDIUM";
        }
        else
        {
            currentDamage = lightDamage;
            currentDamageTier = "LIGHT";
        }

        Debug.Log(
            "DEALT " +
            currentDamage +
            " " +
            currentDamageTier +
            " DAMAGE"
        );

        target.TakeDamage(currentDamage);
    }

    public string GetDamageTier()
    {
        return currentDamageTier;
    }

    public float GetCurrentDamage()
    {
        return currentDamage;
    }
}