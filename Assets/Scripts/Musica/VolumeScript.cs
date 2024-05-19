using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    Slider slider;

    private void Start()
    {
        slider = transform.GetComponent<Slider>();
        slider.value = AudioListener.volume;
    }

    public void ChangeVolume()
    {
        Slider slider = transform.GetComponent<Slider>();
        AudioListener.volume = slider.value;
    }
}
