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

    [Header("Glitch")]
    public float maxMarkerJitter = 40f;
    public float signalLossChance = 0.02f;

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
        // Marker jitter
        //--------------------------------------------------

        float jitter = 0f;

        if (hpPercent <= 0.5f)
        {
            float damageLevel =
                Mathf.InverseLerp(
                    0.5f,
                    0.1f,
                    hpPercent);

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
            Random.value < signalLossChance)
        {
            marker.gameObject.SetActive(false);

            if (distanceText != null)
            {
                distanceText.text =
                    "SIGNAL LOST";
            }

            return;
        }
        else
        {
            marker.gameObject.SetActive(true);
        }

        //--------------------------------------------------
        // Distance text
        //--------------------------------------------------

        if (distanceText != null)
        {
            float distance =
                Vector3.Distance(
                    player.position,
                    objective.position);

            string text =
                Mathf.RoundToInt(distance)
                + "m";

            //--------------------------------------------------
            // Corrupt distance text
            //--------------------------------------------------

            if (hpPercent <= 0.5f)
            {
                float damageLevel =
                    Mathf.InverseLerp(
                        0.5f,
                        0.1f,
                        hpPercent);

                float corruptionChance =
                    Mathf.Lerp(
                        0.1f,
                        0.8f,
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
                        corruptionChance)
                    {
                        chars[i] =
                            GetRandomCharacter();
                    }
                }

                text =
                    new string(chars);
            }

            distanceText.text = text;
        }
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