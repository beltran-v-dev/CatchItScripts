using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class managerTeams : MonoBehaviourPunCallbacks
{
    //Declaració de les variables necessàries.
    public GameObject imgTeams;
    public GameObject rpDgos;
    public GameObject dogPrefab;

    public GameObject rpGat;
    public GameObject gatPrefab;

    private bool gat = false;
    private bool gos = false;

    public TextMeshProUGUI numPlayers;

    //Activem el canvas de la cara del gat i del gos.
    private void Awake()
    {
        imgTeams.SetActive(true);
    }

    //Mètode per assignar el personatge del gos si l'imatge és clicada.
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

    //Mètode per assignar el personatge del gat si l'imatge és clicada.
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

    //Instanciem el prefab del gat a l'escena.
    public void cat()
    {
        PhotonNetwork.Instantiate(gatPrefab.name, rpGat.transform.position, rpGat.transform.rotation);
    }

    //Instanciem el prefab del gos a l'escena.
    public void dog()
    {
        PhotonNetwork.Instantiate(dogPrefab.name, rpDgos.transform.position, rpDgos.transform.rotation);
    }
}