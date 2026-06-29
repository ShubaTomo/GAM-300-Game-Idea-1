using TMPro;
using UnityEngine;

public class SuitStatusUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;

    [Header("Glitch Settings")]
    public float glitchInterval = 2f;
    public float glitchDuration = 0.3f;

    [Range(0f, 1f)]
    public float corruptionChance = 0.1f;

    private TMP_Text statusText;

    private float glitchTimer;
    private float glitchDurationTimer;
    private bool isGlitching;

    private void Start()
    {
        statusText =
            GetComponent<TMP_Text>();

        glitchTimer =
            glitchInterval;
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        float hpPercent =
            playerHealth.currentHealth /
            playerHealth.maxHealth;

        string normalText =
            GetStatusText(
                hpPercent);

        // Healthy
        if (hpPercent > 0.5f)
        {
            statusText.text =
                normalText;

            isGlitching =
                false;

            return;
        }

        float damageLevel =
            Mathf.InverseLerp(
                0.5f,
                0f,
                hpPercent);

        float currentInterval =
            Mathf.Lerp(
                glitchInterval,
                0.15f,
                damageLevel);

        float currentDuration =
            Mathf.Lerp(
                glitchDuration,
                1f,
                damageLevel);

        float currentCorruption =
            Mathf.Lerp(
                corruptionChance,
                0.5f,
                damageLevel);

        if (isGlitching)
        {
            glitchDurationTimer -=
                Time.deltaTime;

            if (glitchDurationTimer <= 0f)
            {
                statusText.text =
                    normalText;

                isGlitching =
                    false;
            }

            return;
        }

        glitchTimer -=
            Time.deltaTime;

        if (glitchTimer <= 0f)
        {
            statusText.text =
                CreateGlitchedText(
                    normalText,
                    currentCorruption);

            isGlitching =
                true;

            glitchDurationTimer =
                currentDuration;

            glitchTimer =
                currentInterval;
        }
        else
        {
            statusText.text =
                normalText;
        }
    }

    private string GetStatusText(
        float hpPercent)
    {
        if (hpPercent > 0.75f)
        {
            return
                "SUIT STATUS\n\n" +
                "VISOR ........ OK\n" +
                "NAVIGATION ... OK\n" +
                "LIFE SUPPORT . OK";
        }
        else if (hpPercent > 0.5f)
        {
            return
                "SUIT STATUS\n\n" +
                "VISOR ........ DAMAGED\n" +
                "NAVIGATION ... OK\n" +
                "LIFE SUPPORT . OK";
        }
        else if (hpPercent > 0.25f)
        {
            return
                "SUIT STATUS\n\n" +
                "VISOR ........ ERROR\n" +
                "NAVIGATION ... DAMAGED\n" +
                "LIFE SUPPORT . OK";
        }
        else
        {
            return
                "SUIT STATUS\n\n" +
                "VISOR ........ FAILURE\n" +
                "NAVIGATION ... OFFLINE\n" +
                "LIFE SUPPORT . CRITICAL\n\n" +
                "WARNING\n" +
                "SYSTEM FAILURE";
        }
    }

    private string CreateGlitchedText(
        string text,
        float corruption)
    {
        char[] chars =
            text.ToCharArray();

        for (int i = 0;
             i < chars.Length;
             i++)
        {
            if (chars[i] == ' ' ||
                chars[i] == '\n')
                continue;

            if (Random.value <
                corruption)
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
            "#@$%!?*";

        return chars[
            Random.Range(
                0,
                chars.Length)];
    }
}