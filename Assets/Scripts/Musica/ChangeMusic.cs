using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] AudioClip newMusic;
    [SerializeField] float newVolume;
    void Start()
    {
        MusicScript.instance.PlayMusic(newMusic, newVolume);
    }
}
