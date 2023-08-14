using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to stop the audio of the menus.
public class pauseAudio : MonoBehaviour
{
    private void Start()
    {
        managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Stop();
    }
}
