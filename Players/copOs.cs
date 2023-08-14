using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copOs : MonoBehaviour
{
    // Variables declaration
    public GameObject osGos;
    public bool tocat = false;

   // Method to check if the dog's object has collided with the player.
    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "PlayerCat")
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
