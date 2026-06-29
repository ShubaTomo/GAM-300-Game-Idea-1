using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlassCrackController : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float currentHP;

    public float opacityValue = 0f;
    public float maxHP = 100f;

    public bool damageTimer = false;
    public float timer;

    public Image[] glassCracks;

    private Coroutine fadeOutCoroutine;

    public void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        foreach (Image glassCrack in glassCracks)
        {
            glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
        }
    }

    public void Update()
    {
        currentHP = playerHealth.currentHealth;

        foreach (Image glassCrack in glassCracks)
        {
            glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartFadeOutCracks();
            }
        }

        if (damageTimer)
        {
            timer = 4f;
            damageTimer = false;
        }
    }

    public float onTakeDamage(float damage)
    {
        float percentage = 1 - (currentHP / maxHP);
        opacityValue = percentage;
        Debug.Log("Opacity Value: " + opacityValue);
        damageTimer = true;

        // If a fade-out is in progress, stop it so the cracks appear instantly on new damage
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
            fadeOutCoroutine = null;
        }

        foreach (Image glassCrack in glassCracks)
        {
            glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
        }

        return 0;
    }

    private void StartFadeOutCracks()
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutCracks());
    }

    private IEnumerator FadeOutCracks()
    {
        float startOpacity = opacityValue;
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            opacityValue = Mathf.Lerp(startOpacity, 0f, elapsed / duration);

            foreach (Image glassCrack in glassCracks)
            {
                glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
            }

            yield return null;
        }

        opacityValue = 0f;
        foreach (Image glassCrack in glassCracks)
        {
            glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
        }

        fadeOutCoroutine = null;
    }
}
