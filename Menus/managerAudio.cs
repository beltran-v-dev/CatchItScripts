using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Classe que ens controlarà que el so no deixi de reproduir-se quan es canviï d'escena als menús.
 */
public class managerAudio : MonoBehaviour
{
   
    private static managerAudio instance = null;

    public static managerAudio Instancia
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}