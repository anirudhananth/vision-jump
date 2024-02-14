using UnityEngine;

public class Utils
{
    public static void Play(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}