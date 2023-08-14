using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Class to control the overall game volume from the options slider.
public class MusicVolum : MonoBehaviour
{
    public AudioMixer mixer;

    public void setLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolum", Mathf.Log10(sliderValue) * 20);
    }
}
