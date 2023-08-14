using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class animdoglocal : MonoBehaviour
{
    // Variables declaration
    public Animator animacio;

    private Rigidbody rb;
    public float vides = 100.0f;

    private int number;
    private float currentTime;
    private float maxTime = 5;

    public Transform target;
    public float distancia;
    public float speed;

    private NavMeshAgent agent;
    public Transform[] points;
    private int destPoint = -1;

    public GameObject cat;
    public GameObject gos;
    private bool possibleImpacte = false;
    private float contador;

    public GameObject barLife;
    public GameObject osGos;
    private bool gatTocat;
    private bool animGolpej = false;

    /**
     * We search for the corresponding components before the Update loop execution.
     */
    private void Start()
    {
        animacio = GetComponent<Animator>();        
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("PlayerCat").GetComponent<Transform>();

    }

    
    private void Update()
    {
        //We retrieve the value of the variable from the copOs script and update the UI..
        gatTocat = osGos.GetComponent<copOs>().tocat;
        barLife.GetComponent<Image>().fillAmount = vides / 100f;
        
        //We calculate the distance between the AI and the player.
        distancia = Vector3.Distance(target.position, transform.position);

     // If it is within a distance less than the one we have assigned, it will move towards the player; otherwise, it will follow the designated route.
        if (distancia <= 10)
        {          
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // We make it look towards the player.
            if (target != null)
            {
                transform.LookAt(target);
            }

       // If it is within this distance, the hitting animation is played.
            if (distancia < 3)
            {
                animacio.SetBool("golpejar", true);
                animGolpej = true;
            }
            else
            {
                animacio.SetBool("golpejar", false);
                animGolpej = false;
            }           
        }
        // If the player picks up the object, we increase their speed.
        else if (cat.GetComponent<animcatlocal>().getTeObjecte() == true)
        {
            speed = 10;
        }
     // Return of the established route.
        else
        {
      // The next destination point is chosen when the agent approaches the current one.

            speed = 8;
            if (agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }

            animacio.SetBool("golpejar", false);
        }

    // Management to prevent the AI from abusing the hitting system and not being able to cause damage too frequently.
        if (possibleImpacte == false)
        {
            contador += Time.deltaTime * 1;            
            if (contador >= 3.0f)
            {
                possibleImpacte = true;
                contador = 0f;
            }
        }

        // If the AI is allowed to perform the attack, has executed the attack, and the impact has been successful, we decrease the player's health and perform the animation.
        if (gatTocat && possibleImpacte && animGolpej)
        {
            cat.GetComponent<animcatlocal>().playImpacte();
            cat.GetComponent<animcatlocal>().setVides(1);
            animacio.SetBool("impacte", true);
            possibleImpacte = false;
        }
        else
        {
            animacio.SetBool("impacte", false);
        }
    }

    // Method that iterates through the array of objects marking the path of the AI.
    private void GotoNextPoint()
    {
        // Returns if no point has been configured.
        destPoint++;

        if (destPoint < points.Length)
        {
            agent.destination = points[destPoint].position;
        }
        else
        {
            destPoint = -1;
        }
    }

    // Getter to get the lives of the AI.
    public float getVides()
    {
        return vides;
    }

   // Setter to assign the lives of the AI.
    public void setVides(float i)
    {
        vides -= i * 20;       
    }

  // Method to assign lives to the AI.
    public void respawn()
    {
        vides = 100;
    }
}
