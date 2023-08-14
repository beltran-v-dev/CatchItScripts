using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class managerTeams : MonoBehaviourPunCallbacks
{
    //Variables declaration
    public GameObject imgTeams;
    public GameObject rpDgos;
    public GameObject dogPrefab;

    public GameObject rpGat;
    public GameObject gatPrefab;

    private bool gat = false;
    private bool gos = false;

    public TextMeshProUGUI numPlayers;

// We activate the canvas of the cat and dog's face.
    private void Awake()
    {
        imgTeams.SetActive(true);
    }

 // Method to assign the dog character if the image is clicked.

    public void OnClick_DogTeam()
    {
        imgTeams.SetActive(false);

        gos = true;
        gat = false;

        if (gos == true && gat == false)
        {
            dog();
        }
    }

// Method to assign the cat character if the image is clicked.

    public void OnClick_CatTeam()
    {
        // Time.timeScale = 1;
        imgTeams.SetActive(false);

        gat = true;
        gos = false;

        if (gos == false && gat == true)
        {
            cat();
        }
    }

// We instantiate the cat prefab in the scene.
    public void cat()
    {
        PhotonNetwork.Instantiate(gatPrefab.name, rpGat.transform.position, rpGat.transform.rotation);
    }

   // We instantiate the dog prefab in the scene.
    public void dog()
    {
        PhotonNetwork.Instantiate(dogPrefab.name, rpDgos.transform.position, rpDgos.transform.rotation);
    }
}
