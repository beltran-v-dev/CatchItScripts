using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Declaració de les variables necessàries.
    public float densitatBoira1;
    public float densitatBoira2;
    public Color colorBoira1;
    public Color colorBoira2;
    public GameObject player;

    private float numVides;
    private bool zona = false;
    private float contador = 1.0f;

    //Obtenim les vides del gat i els i restem mitjançant el Time.deltaTime perquè sigui de forma continuada.
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

    //Control per saber si el jugador ha entrat a la zona de joc.
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

    //Control per saber si el jugador ha sortit de la zona de joc i s'aplica tant el renderitzat de la boira per advertir-lo com la reducció de la vida.
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