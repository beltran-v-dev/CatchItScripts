using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class to load the scene for offline mode.
public class offline : MonoBehaviour
{
    public void gameOffline()
    {
        SceneManager.LoadScene("terreny_v4");
    }
}
