using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Declaration of the necessary variables.
    public float densitatBoira1;
    public float densitatBoira2;
    public Color colorBoira1;
    public Color colorBoira2;
    public GameObject player;

    private float numVides;
    private bool zona = false;
    private float contador = 1.0f;

    //We get the lives of the cat and subtract them using Time.deltaTime to make it continuous.
    private void Update()
    {
        numVides = player.GetComponent<animcatlocal>().getVides();

        if (!zona)
        {
            contador -= Time.deltaTime;
            player.GetComponent<animcatlocal>().setVides(1 * Time.deltaTime);
            contador = 1.0f;
        }
    }

    //Check to determine if the player has entered the game area.
    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Zona")
        {
            zona = true;
            RenderSettings.fog = false;
            RenderSettings.fogColor = colorBoira2;
            RenderSettings.fogDensity = densitatBoira2;
        }
    }

    //Check to determine if the player has exited the game area, applying both the rendering of fog to warn them and the reduction of life.
    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Zona")
        {
            zona = false;
            RenderSettings.fog = true;
            RenderSettings.fogColor = colorBoira1;
            RenderSettings.fogDensity = densitatBoira1;
        }
    }
}
