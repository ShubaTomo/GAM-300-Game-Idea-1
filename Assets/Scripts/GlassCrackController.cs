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
                opacityValue = 0f;
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
        opacityValue = 250 * percentage;
        Debug.Log("Opacity Value: " + opacityValue);
        damageTimer = true;
        return 0;
    }

    //IEnumerator IFadeOutCracks()
    //{
    //    while (opacityValue > 0)
    //    {
    //        opacityValue -= Time.deltaTime * 10f;

    //        Debug.Log("Opacity Value: " + opacityValue);

    //        foreach (Image glassCrack in glassCracks)
    //        {
    //            glassCrack.color = new Color(glassCrack.color.r, glassCrack.color.g, glassCrack.color.b, opacityValue);
    //        }

    //        yield return null;
    //    }
    //    opacityValue = 0f;
    //}
}
