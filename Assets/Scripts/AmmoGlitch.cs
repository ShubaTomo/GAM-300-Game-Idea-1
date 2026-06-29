using TMPro;
using UnityEngine;

public class AmmoGlitch : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerShooting playerShooting;

    [Header("Glitch Settings")]
    public float glitchInterval = 2f;
    public float glitchDuration = 0.3f;

    private TMP_Text ammoText;

    private float glitchTimer;
    private float glitchDurationTimer;

    private bool isGlitching;

    private void Start()
    {
        ammoText = GetComponent<TMP_Text>();

        glitchTimer = glitchInterval;
    }

    private void Update()
    {
        if (playerHealth == null ||
            playerShooting == null)
            return;

        //--------------------------------------------------
        // Figure out what the normal text should be
        //--------------------------------------------------

        string normalText;

        if (playerShooting.IsReloading)
        {
            int dots =
                (int)(Time.time * 3f) % 4;

            normalText =
                "RELOAD" +
                new string('.', dots);
        }
        else if (
            playerShooting.currentAmmo <= 0 &&
            playerShooting.reserveAmmo <= 0)
        {
            normalText = "EMPTY";
        }
        else
        {
            normalText =
                playerShooting.currentAmmo +
                " | " +
                playerShooting.reserveAmmo;
        }

        float hpPercent =
            playerHealth.currentHealth /
            playerHealth.maxHealth;

        //--------------------------------------------------
        // Healthy
        //--------------------------------------------------

        if (hpPercent > 0.5f)
        {
            ammoText.text = normalText;
            isGlitching = false;
            return;
        }

        //--------------------------------------------------
        // Damage amount
        //--------------------------------------------------

        float damageLevel =
            Mathf.InverseLerp(
                0.5f,
                0.1f,
                hpPercent);

        float currentInterval =
            Mathf.Lerp(
                glitchInterval,
                0.25f,
                damageLevel);

        float currentDuration =
            Mathf.Lerp(
                glitchDuration,
                1f,
                damageLevel);

        //--------------------------------------------------
        // Currently glitching
        //--------------------------------------------------

        if (isGlitching)
        {
            glitchDurationTimer -=
                Time.deltaTime;

            if (glitchDurationTimer <= 0f)
            {
                ammoText.text =
                    normalText;

                isGlitching = false;
            }

            return;
        }

        //--------------------------------------------------
        // Wait for next glitch
        //--------------------------------------------------

        glitchTimer -= Time.deltaTime;

        if (glitchTimer <= 0f)
        {
            ammoText.text =
                CreateGlitchedAmmo(
                    normalText,
                    damageLevel);

            isGlitching = true;

            glitchDurationTimer =
                currentDuration;

            glitchTimer =
                currentInterval;
        }
    }

    private string CreateGlitchedAmmo(
        string text,
        float damageLevel)
    {
        char[] chars =
            text.ToCharArray();

        float corruptionChance =
            Mathf.Lerp(
                0.05f,
                0.9f,
                damageLevel);

        for (int i = 0;
            i < chars.Length;
            i++)
        {
            if (chars[i] == ' ')
                continue;

            if (Random.value <
                corruptionChance)
            {
                chars[i] =
                    GetRandomCharacter();
            }
        }

        return new string(chars);
    }

    private char GetRandomCharacter()
    {
        string chars =
            "#@$%!?*0123456789";

        return chars[
            Random.Range(
                0,
                chars.Length)];
    }
}