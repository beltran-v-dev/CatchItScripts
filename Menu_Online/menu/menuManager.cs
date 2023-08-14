using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class menuManager : MonoBehaviourPunCallbacks
{
    //Variables declaration

    [SerializeField]
    private GameObject userNameScreen, ConnectScreen, screenTeam, loadScreen, background;

    [SerializeField]
    private GameObject createUserNameButton;

    [SerializeField]
    private InputField userNaemInput, createRooomInput, joinRoomInput;

    [SerializeField]
    private GameObject textConnection;

    private bool conect = true;


    private void Awake()
    {
        //Desactivem el botó del nom, més endavant controlem aquesta qüestió.

        createUserNameButton.GetComponent<Button>().interactable = false;
    }

    /**
     * Mètode sobreescrit per tal de crear una connexió cap al servidor de photon
     */

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /**
     * Mètode sobreescrit per tal d'unirse a la sala photon
     */

    public override void OnJoinedLobby()
    {
        textConnection.SetActive(false);
        userNameScreen.SetActive(true);
    }

    /***
     * Mètode en el qual activem la pantalla corresponent, perquè els usuaris puguin inserir els seus noms.
     */

    public void OnClik_CreateNameBtn()
    {
        PhotonNetwork.NickName = userNaemInput.text;
        userNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
        loadScreen.SetActive(false);
    }

    /**
     * Mètode en el qual controlem si el jugador pot avançar o no, de tal forma que el nom tingui una mida correcta.
     */

    public void OnNameField_Changed()
    {
        if (userNaemInput.text.Length >= 1)
        {
            createUserNameButton.GetComponent<Button>().interactable = true;
        }
        else if (userNaemInput.text.Length < 1)
        {
            createUserNameButton.GetComponent<Button>().interactable = false;
        }
    }

    /**
     * Amb el següent mètode, s'indica al projecte d'Unity, que ha de fer servir l'arxiu de configuració que s'ha realitzat prèviament.
     *
     */

    public void OnClik_BtnOnline()
    {
        PhotonNetwork.ConnectUsingSettings();

        textConnection.SetActive(true);
        loadScreen.SetActive(false);
    }

    /**
     *  Mètode per a unir-se a l'escena corresponent.
     */

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    /**
     *Mètode per a crear una sala al servidor de photon, amb un nom concret i quants jugadors poden accedir com a màxim.
     */

    public void OnClick_CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRooomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
        screenTeam.SetActive(true);
        userNameScreen.SetActive(false);
        ConnectScreen.SetActive(false);
        loadScreen.SetActive(false);
        background.SetActive(false);
        PhotonNetwork.LoadLevel("SampleScene");
    }

    /**
     * Mètode per a unir-se a la sala creada prèviament.
     */

    public void OnClick_JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
        screenTeam.SetActive(false);
        userNameScreen.SetActive(false);
        ConnectScreen.SetActive(false);
        loadScreen.SetActive(false);
        background.SetActive(false);
    }

    /**
     * Mètode que indica un error si no hi ha una sala creada i intentem unir-se.
     */

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        conect = false;
    }
}
