using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Classe per controlar el volum general del joc des de l'slider de les opcions.
public class MusicVolum : MonoBehaviour
{
    public AudioMixer mixer;

    public void setLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolum", Mathf.Log10(sliderValue) * 20);
    }
}