using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 Class that will control the sound to keep playing when switching scenes to the menus.

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
