using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Gun Sounds")]

    public AudioClip[] GunSFX;

    [Header("Player Sounds")]

    public AudioClip[] PlayerSFX;

    [Header("Other Sounds")]

    public AudioClip[] OtherSFX;

    [Header("Audio Sources")]

    public AudioSource playerMovementAudioSource;
    public AudioSource playerGunSource;

    private void Start()
    {
        playerMovementAudioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerSFXByIndex(int index)
    {
        if (playerMovementAudioSource.isPlaying)
        {
            return;
        }
        playerMovementAudioSource.PlayOneShot(PlayerSFX[index]);
    }
    public void PlayGunSFXByIndex(int index)
    {
        if (playerGunSource.isPlaying)
        {
            return;
        }
        playerGunSource.PlayOneShot(GunSFX[index]);
    }

    public void StopShootingSFX()
    {
        Debug.Log("Stopping shooting sound");
        playerGunSource.Stop();
        playerGunSource.PlayOneShot(GunSFX[1]);
    }

    public void StopPlayerSFX()
    {
        Debug.Log("Stopping player movement sound");
        playerMovementAudioSource.Stop();
    }
}
