using TMPro;
using UnityEngine;

public class ObjectiveCompass : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform objective;

    public RectTransform marker;
    public TMP_Text distanceText;

    public PlayerHealth playerHealth;

    [Header("Compass")]
    public float compassWidth = 500f;

    [Header("Glitch Settings")]
    public float glitchInterval = 1f;
    public float glitchDuration = 0.35f;

    public float maxMarkerJitter = 40f;

    private float glitchTimer;
    private float glitchDurationTimer;
    private bool isGlitching;

    private void Start()
    {
        glitchTimer =
            glitchInterval;
    }

    private void Update()
    {
        if (player == null ||
            objective == null ||
            marker == null ||
            playerHealth == null)
            return;

        float hpPercent =
            playerHealth.currentHealth /
            playerHealth.maxHealth;

        //--------------------------------------------------
        // Calculate marker position
        //--------------------------------------------------

        Vector3 direction =
            objective.position -
            player.position;

        direction.y = 0f;

        float angle =
            Vector3.SignedAngle(
                player.forward,
                direction,
                Vector3.up);

        float xPos =
            (angle / 180f) *
            (compassWidth / 2f);

        xPos = Mathf.Clamp(
            xPos,
            -compassWidth / 2f,
            compassWidth / 2f);

        //--------------------------------------------------
        // Glitch timer
        //--------------------------------------------------

        if (hpPercent > 0.5f)
        {
            isGlitching = false;

            marker.gameObject.SetActive(true);

            marker.anchoredPosition =
                new Vector2(
                    xPos,
                    marker.anchoredPosition.y);

            UpdateDistanceText(
                hpPercent,
                false);

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

        if (isGlitching)
        {
            glitchDurationTimer -=
                Time.deltaTime;

            if (glitchDurationTimer <= 0f)
            {
                isGlitching =
                    false;
            }
        }
        else
        {
            glitchTimer -=
                Time.deltaTime;

            if (glitchTimer <= 0f)
            {
                isGlitching =
                    true;

                glitchDurationTimer =
                    currentDuration;

                glitchTimer =
                    currentInterval;
            }
        }

        //--------------------------------------------------
        // Marker jitter
        //--------------------------------------------------

        float jitter = 0f;

        if (isGlitching)
        {
            jitter =
                Random.Range(
                    -maxMarkerJitter,
                    maxMarkerJitter)
                * damageLevel;
        }

        marker.anchoredPosition =
            new Vector2(
                xPos + jitter,
                marker.anchoredPosition.y);

        //--------------------------------------------------
        // Critical signal loss
        //--------------------------------------------------

        if (hpPercent <= 0.1f &&
            isGlitching)
        {
            marker.gameObject.SetActive(
                false);

            if (distanceText != null)
            {
                distanceText.text =
                    "SIGNAL LOST";
            }

            return;
        }
        else
        {
            marker.gameObject.SetActive(
                true);
        }

        //--------------------------------------------------
        // Distance text
        //--------------------------------------------------

        UpdateDistanceText(
            hpPercent,
            isGlitching);
    }

    private void UpdateDistanceText(
        float hpPercent,
        bool glitched)
    {
        if (distanceText == null)
            return;

        float distance =
            Vector3.Distance(
                player.position,
                objective.position);

        string text =
            Mathf.RoundToInt(
                distance)
            + "m";

        if (glitched)
        {
            float damageLevel =
                Mathf.InverseLerp(
                    0.5f,
                    0f,
                    hpPercent);

            float currentCorruption =
                Mathf.Lerp(
                    0.1f,
                    0.5f,
                    damageLevel);

            char[] chars =
                text.ToCharArray();

            for (int i = 0;
                 i < chars.Length;
                 i++)
            {
                if (chars[i] == 'm')
                    continue;

                if (Random.value <
                    currentCorruption)
                {
                    chars[i] =
                        GetRandomCharacter();
                }
            }

            text =
                new string(chars);
        }

        distanceText.text =
            text;
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