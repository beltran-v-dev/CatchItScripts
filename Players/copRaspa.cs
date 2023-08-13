using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class copRaspa : MonoBehaviour
{
    // Declaració de les variables necessàries.
    public GameObject raspapeix;
    public bool tocat = false;

    //Mètode per comprovar si l'objecte del gat ha impactat amb la IA.
    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "PlayerDog")
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
