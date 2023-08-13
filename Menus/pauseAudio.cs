using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe per aturar l'audio dels menús.
public class pauseAudio : MonoBehaviour
{
    private void Start()
    {
        managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Stop();
    }
}