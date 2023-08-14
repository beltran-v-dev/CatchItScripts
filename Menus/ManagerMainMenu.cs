using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
Class to control the actions performed in the main menu.
 */
public class ManagerMainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject menuControls;
    public GameObject menuOptions;
    public GameObject menuCredits;

    private void Start()
    {
        mainMenu.SetActive(true);
        menuControls.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene("ConnectName");
    }

    public void onClickControls()
    {
        mainMenu.SetActive(false);
        menuControls.SetActive(true);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
    }

    public void onClickBackMainMenu()
    {
        mainMenu.SetActive(true);
        menuControls.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
    }

    public void onClickOptions()
    {
        menuOptions.SetActive(true);
        mainMenu.SetActive(false);
        menuControls.SetActive(false);
        menuCredits.SetActive(false);
    }

    public void onClickCredits()
    {
        menuOptions.SetActive(false);
        mainMenu.SetActive(false);
        menuControls.SetActive(false);
        menuCredits.SetActive(true);
    }

    public void onClickExit()
    {
        Application.Quit();
    }
}
