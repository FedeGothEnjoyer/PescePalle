using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource audioSource;
    public static MusicScript instance;

    private void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip music, float setVolume)
    {
        if (music == audioSource.clip)
            return;
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.volume = setVolume;
        audioSource.Play();
    }
}