using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class animcatlocal : MonoBehaviour
{
    // Declaració de les variables necessàries.
    public Animator animacio;

    private Rigidbody rb;
    public float vides = 100.0f;

    private int number;
    private float currentTime;
    private float maxTime = 5;
    public float speedPlayer;

    public GameObject raspapeix;
    public GameObject arma;

    public Camera cam;

    public float h;
    public float v;
    public float horizontalSpeed;
    public float verticalSpeed;

    public Transform target;
    public float distancia;

    private int puntuacio = 0;
    private int puntuacioIa = 0;
    private bool tincOs = false;
    public GameObject os;
    public GameObject barLife;
    public GameObject objecteUI;
    public TextMeshProUGUI txtPuntuacio;
    public GameObject dog;
    public GameObject cat;
    private bool possibleImpacte = false;
    private float contador;
    private bool dogDeath = false;
    private float videsDog;

    private float time = 2.0f;
    private Vector3 posDogInic;
    private Vector3 posCatInic;
    private Quaternion rotCatInic;
    private bool gosTocat;
    private bool animGat = false;

    public AudioSource runGrass;
    public AudioSource runWood;
    public AudioSource water;
    public AudioSource defeatAS;
    public AudioSource winSound;
    public AudioSource atackSound;
    public AudioSource damageLifeSound;
    public AudioSource objectBone;
    public AudioSource objectPoint;

    public bool moviment = false;

    public bool touchingGrass = false;
    public bool touchingWood = false;

    private float timer, timer2, timerDefeat;
    private float timeBetweenShots, timeBetweenShots2, timeDefeat;
    public AudioClip runGrassClip, runWoodClip, waterClip, defeatClip;

    private bool reproduitLose = false;
    private bool reproduitWin = false;
    private bool reproduitAtack = false;

    public GameObject rawImage;
    public GameObject videoCatWin;
    public GameObject videoCatLose;

    /**
     * Busquem els components corresponents abans de l'execució en bucle de l'Update i associem els temps dels audios a les variables.
     */
    private void Start()
    {
        animacio = GetComponent<Animator>();
        posDogInic = dog.GetComponent<Transform>().position;
        posCatInic = cat.GetComponent<Transform>().position;
        rotCatInic = cat.GetComponent<Transform>().rotation;
       
        timeBetweenShots = runGrassClip.length;
        timeBetweenShots2 = runWoodClip.length;
        timeDefeat = defeatClip.length;
    }

    private void Update()
    {
        //Obtenim el valor de la variable de l'script de la raspa, assignem l'objecte raspa i actualitzem la UI.
        gosTocat = raspapeix.GetComponent<copRaspa>().tocat;
        raspapeix = GameObject.FindGameObjectWithTag("cat");
        barLife.GetComponent<Image>().fillAmount = vides / 100f;

        //Controlem el moviment de la càmera.
        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, h, 0);
        cam.transform.Rotate(-v, 0, 0);

        //Controlem el moviment del jugador.
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");
        transform.Translate(xAxis * speedPlayer * Time.deltaTime, 0, zAxis * speedPlayer * Time.deltaTime);

        //Controlem si el jugador s'està movent i parem el so i l'animació quan està quiet.
        if (zAxis != 0 || xAxis != 0)
        {
            animacio.SetBool("vertical_b", true);
            moviment = true;
        }
        else
        {
            animacio.SetBool("vertical_b", false);

            runGrass.Stop();
            runWood.Stop();
            moviment = false;
        }

        //Depenent de la superfície que estigui tocant reproduirem un audio o un altre.
        if (touchingGrass)
        {
            runWood.Stop();

            timer += Time.deltaTime;
            if (timer > timeBetweenShots)
            {
                runGrass.PlayOneShot(runGrassClip);
                timer = 0;
            }
        }
        else
        {
            runGrass.Stop();

            timer2 += Time.deltaTime;
            if (timer2 >= timeBetweenShots2)
            {
                runWood.PlayOneShot(runWoodClip);
                timer2 = 0;
            }
        }

        //Realitzem el control de les animacions i del so de la mecànica de colpejar.
        try
        {
            if (Input.GetKeyDown(KeyCode.G) && raspapeix.activeSelf)
            {
                animacio.SetBool("golpejar", true);

                animGat = true;

                if (!reproduitAtack)
                {
                    atackSound.Play();
                    reproduitAtack = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.G))
            {
                animacio.SetBool("golpejar", false);
                animGat = false;

                reproduitAtack = false;
                atackSound.Stop();
            }
        }
        catch
        {
            Debug.Log("No té la arma per golpejar");
        }

        //Control de si el jugador ha perdut la partida arribant a 0 vides, o de si l'ha guanyat al arribar als 3 punts,
        //reproduint els sons i videos corresponents, i permetem el retorn al menú principal.
        if (vides <= 0f)
        {
            if (!reproduitLose)
            {
                defeatAS.Play();
                reproduitLose = true;
            }

            rawImage.SetActive(true);
            videoCatWin.SetActive(false);
            videoCatLose.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rawImage.SetActive(false);
                videoCatLose.SetActive(false);
                videoCatWin.SetActive(false);

                SceneManager.LoadScene("MainMenu");
            }

            tincOs = false;
            objecteUI.SetActive(false);
            os.SetActive(true);      

        }
        else if (puntuacio == 3)
        {
            rawImage.SetActive(true);
            videoCatWin.SetActive(true);
            videoCatLose.SetActive(false);

            if (!reproduitWin)
            {
                winSound.Play();
                reproduitWin = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rawImage.SetActive(false);
                videoCatLose.SetActive(false);
                videoCatWin.SetActive(false);

                SceneManager.LoadScene("MainMenu");
            }
        }

        //Control perquè el jugador no abusi del sistema de colpejar i no es pugui realitzar mal tant seguidament.
        if (possibleImpacte == false)
        {
            contador += Time.deltaTime * 1;            
            if (contador >= 3.0f)
            {
                possibleImpacte = true;
                contador = 0f;
            }
        }

        //Obtenim les vides de la IA i si és 0 el tornem a respawnejar.
        videsDog = dog.GetComponent<animdoglocal>().getVides();
        if (videsDog == 0)
        {
            dog.SetActive(false);
            time -= Time.deltaTime;

            if (time <= 0)
            {
                dog.SetActive(true);
                dog.GetComponent<animdoglocal>().respawn();
                dog.GetComponent<Transform>().position = posDogInic;
            }            
        }

        //Si el jugador se li permet realitzar l'impacte, ha pitjat el botó d'atac ("G") i l'impacte s'ha realitzat li restem vida a la IA i es fa l'animació.
        if (gosTocat && possibleImpacte && animGat)
        {
            dog.GetComponent<animdoglocal>().setVides(1);
            animacio.SetBool("impacte", true);
            possibleImpacte = false;
        }
        else
        {
            animacio.SetBool("impacte", false);
        }

        //Input per retornar al menú principal.
        if (Input.GetKey(KeyCode.Escape))
        {
            managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("MainMenu");
        }
    }

    //Mètode que comprova si el gat ha entrat en col·lissió amb diversos elements del joc, 
    //com l'objecte del gos, l'altar de la seva base i l'aigua i els aplica les accions a dur a terme corresponents.
    public void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.tag == "DogBone")
        {
            objecteUI.SetActive(true);
            tincOs = true;
            os.SetActive(false);
            objectBone.Play();
        }
        else if (collision.gameObject.tag == "CatZone" && tincOs == true)
        {
            puntuacio++;
            objecteUI.SetActive(false);
            txtPuntuacio.SetText("" + puntuacio);
            vides += 20;
            tincOs = false;
            os.SetActive(true);
            objectPoint.Play();
        }

        if (collision.gameObject.tag == "Water")
        {
            water.PlayOneShot(waterClip);
            tincOs = false;
            objecteUI.SetActive(false);
            os.SetActive(true);
            cat.GetComponent<Transform>().position = posCatInic;
            cat.GetComponent<Transform>().rotation = rotCatInic;
        }
    }

    //Mètode que ens comprova si el gat està tocant la gespa o el pont.
    public void OnCollisionStay(Collision collision) 
    {
        if (collision.gameObject.tag == "floor" && moviment)
        {
            touchingGrass = true;
            touchingWood = false;
        }
        else if (moviment && collision.gameObject.name == "Pont_centre" || collision.gameObject.name == "Pont_esquerra" || collision.gameObject.name == "Pont_dreta")
        {            
            touchingWood = true;
            touchingGrass = false;
        }

    }

    //Getter per obtenir les vides del gat.
    public float getVides()
    {
        return vides;
    }

    //Setter per assignar les vides al gat.
    public void setVides(float i)
    {
        vides -= i * 20;        
    }

    //Mètode per comprovar si el gat té l'objecte agafat o no.
    public bool getTeObjecte()
    {
        return tincOs;
    }

    //Mètode per assignar les vides al gat.
    public void respawn()
    {
        vides = 100;
    }

    //Mètode per reproduir el so d'impacte.
    public void playImpacte()
    {
        damageLifeSound.Play();
    }
}