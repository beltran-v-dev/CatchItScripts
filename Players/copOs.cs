using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copOs : MonoBehaviour
{
    // Declaració de les variables necessàries.
    public GameObject osGos;
    public bool tocat = false;

    //Mètode per comprovar si l'objecte del gos ha impactat amb el jugador.
    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "PlayerCat")
        {
            tocat = true;
        }
    }

    //Retornem la variable a false en cas de no estar-lo tocant. 
    private void OnTriggerExit(Collider other)
    {
        tocat = false;
    }
}