using TMPro;
using UnityEngine;

public class TextGlitch : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;

    [Header("Glitch Settings")]
    public float glitchInterval = 3f;
    public float glitchDuration = 0.2f;

    [Header("Critical Failure")]
    public float errorMessageInterval = 2f;

    private TMP_Text objectiveText;
    private string originalText;

    private float glitchTimer;
    private float glitchDurationTimer;
    private float errorTimer;

    private bool isGlitching;

    private void Start()
    {
        objectiveText =
            GetComponent<TMP_Text>();

        originalText =
            objectiveText.text;

        glitchTimer =
            glitchInterval;

        errorTimer =
            errorMessageInterval;
    }

    private void Update()
    {
        float hpPercent =
            playerHealth.currentHealth /
            playerHealth.maxHealth;

        // Critical failure state
        if (hpPercent <= 0.1f)
        {
            ShowCriticalFailure();
            return;
        }

        // Healthy
        if (hpPercent > 0.5f)
        {
            objectiveText.text =
                originalText;

            isGlitching =
                false;

            return;
        }

        float damageLevel =
            Mathf.InverseLerp(
                0.5f,
                0.1f,
                hpPercent);

        float currentInterval =
            Mathf.Lerp(
                glitchInterval,
                0.5f,
                damageLevel);

        float currentDuration =
            Mathf.Lerp(
                glitchDuration,
                0.6f,
                damageLevel);

        if (isGlitching)
        {
            glitchDurationTimer -=
                Time.deltaTime;

            if (glitchDurationTimer <= 0)
            {
                isGlitching =
                    false;

                objectiveText.text =
                    originalText;
            }

            return;
        }

        glitchTimer -=
            Time.deltaTime;

        if (glitchTimer <= 0)
        {
            objectiveText.text =
                CreateGlitchedText(
                    originalText,
                    damageLevel);

            isGlitching =
                true;

            glitchDurationTimer =
                currentDuration;

            glitchTimer =
                currentInterval;
        }
    }

    private string CreateGlitchedText(
        string text,
        float damageLevel)
    {
        char[] chars =
            text.ToCharArray();

        float corruptionChance =
            Mathf.Lerp(
                0.1f,
                0.8f,
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
                    GetRandomGlitchCharacter();
            }
        }

        return new string(chars);
    }

    private char GetRandomGlitchCharacter()
    {
        string glitchChars =
            "#@$%&!?*0123456789";

        return glitchChars[
            Random.Range(
                0,
                glitchChars.Length)];
    }

    private void ShowCriticalFailure()
    {
        string[] errors =
        {
            "SIGNAL LOST",
            "OBJECTIVE DATA CORRUPTED",
            "CONNECTION FAILED",
            "HUD FAILURE",
            "ERROR 404",
            "NO SIGNAL",
            "RECONNECTING...",
            "OBJECTIVE UNKNOWN"
        };

        errorTimer -=
            Time.deltaTime;

        if (errorTimer <= 0f)
        {
            objectiveText.text =
                errors[
                    Random.Range(
                        0,
                        errors.Length)];

            errorTimer =
                errorMessageInterval;
        }
    }
}