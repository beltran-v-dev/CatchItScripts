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
        //We deactivate the name button, later we will manage this issue

        createUserNameButton.GetComponent<Button>().interactable = false;
    }

    /**
     * Overridden method to establish a connection to the Photon server.
     */

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /**
     * Overridden method to join the Photon room.
     */

    public override void OnJoinedLobby()
    {
        textConnection.SetActive(false);
        userNameScreen.SetActive(true);
    }

    /***
     * Method in which we activate the corresponding screen so that users can input their names.
     */

    public void OnClik_CreateNameBtn()
    {
        PhotonNetwork.NickName = userNaemInput.text;
        userNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);
        loadScreen.SetActive(false);
    }

    /**
     * Method in which we control whether the player can proceed or not, ensuring that the name has a correct length.
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
     * With the following method, Unity project is instructed to use the configuration file that was previously created.
     *
     */

    public void OnClik_BtnOnline()
    {
        PhotonNetwork.ConnectUsingSettings();

        textConnection.SetActive(true);
        loadScreen.SetActive(false);
    }

    /**
     *  Method to join the corresponding scene.
     */

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    /**
     *Method to create a room on the Photon server with a specific name and maximum number of players allowed.
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
     * Method to join the previously created room.
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
     * Method that displays an error if there is no room created and we try to join.
     */

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        conect = false;
    }
}
