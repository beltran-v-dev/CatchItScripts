using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Classe per carregar l'escena del mode offline.
public class offline : MonoBehaviour
{
    public void gameOffline()
    {
        SceneManager.LoadScene("terreny_v4");
    }
}