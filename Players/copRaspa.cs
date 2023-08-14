using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class copRaspa : MonoBehaviour
{
    // Variables declaration
    public GameObject raspapeix;
    public bool tocat = false;

   // Method to check if the cat's object has collided with AI

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "PlayerDog")
        {
            tocat = true;
        }
    }

// We return the variable to false if it's not being touched.
    private void OnTriggerExit(Collider other)
    {
        tocat = false;
    }
}
