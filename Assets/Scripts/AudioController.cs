using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] Slider[] volumeSliders;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
        foreach(Slider slider in volumeSliders)
        {
            slider.value = audioSource.volume;
        }
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        audioSource.volume = value;
        PlayerPrefs.Save();
    }
}
