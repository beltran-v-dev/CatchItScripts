using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class animdoglocal : MonoBehaviour
{
    // Declaració de les variables necessàries.
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
     * Busquem els components corresponents abans de l'execució en bucle de l'Update.
     */
    private void Start()
    {
        animacio = GetComponent<Animator>();        
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("PlayerCat").GetComponent<Transform>();

    }

    
    private void Update()
    {
        //Obtenim el valor de la variable de l'script de l'os i actualitzem la UI.
        gatTocat = osGos.GetComponent<copOs>().tocat;
        barLife.GetComponent<Image>().fillAmount = vides / 100f;
        
        //Calculem la distància entre la IA i el jugador.
        distancia = Vector3.Distance(target.position, transform.position);

        //Si es troba a una distància menor a la que li hem assignat es dirigirà cap al jugador, i sino, seguirà la ruta designada.
        if (distancia <= 10)
        {          
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            //Fem que miri cap al jugador.
            if (target != null)
            {
                transform.LookAt(target);
            }

            //Si es troba en aquesta distància es reprodueix l'animació de colpejar.
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
        //Si el jugador agafa l'objecte li augmentem la velocitat.
        else if (cat.GetComponent<animcatlocal>().getTeObjecte() == true)
        {
            speed = 10;
        }
        //Retorn de la ruta establerta.
        else
        {
            // Es tria el punt de destinació següent quan l’agent s’acosti a l’actual.
            speed = 8;
            if (agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }

            animacio.SetBool("golpejar", false);
        }

        //Control perquè la IA no abusi del sistema de colpejar i no es pugui realitzar mal tant seguidament.
        if (possibleImpacte == false)
        {
            contador += Time.deltaTime * 1;            
            if (contador >= 3.0f)
            {
                possibleImpacte = true;
                contador = 0f;
            }
        }

        //Si la IA se li permet realitzar l'impacte, ha realitzat l'atac i l'impacte s'ha realitzat correctament li restem vida al jugador i es fa l'animació.
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

    //Mètode que recorre l'array dels objectes que marquen el recorregut de la IA.
    private void GotoNextPoint()
    {
        // Retorna si no s’ha configurat cap punt.
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

    //Getter per obtenir les vides de la IA.
    public float getVides()
    {
        return vides;
    }

    //Setter per assignar les vides de la IA.
    public void setVides(float i)
    {
        vides -= i * 20;       
    }

    //Mètode per assignar les vides a la IA.
    public void respawn()
    {
        vides = 100;
    }
}