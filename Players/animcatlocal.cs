using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class animcatlocal : MonoBehaviour
{
    //Variables declaration
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
     * We search for the corresponding components before the Update loop execution and associate the audio times with the variables.
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
        //We get the value of the variable from the "raspa" script, assign the "raspa" object, and update the UI.
        gosTocat = raspapeix.GetComponent<copRaspa>().tocat;
        raspapeix = GameObject.FindGameObjectWithTag("cat");
        barLife.GetComponent<Image>().fillAmount = vides / 100f;

        //We manage the movement of the camera.
        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, h, 0);
        cam.transform.Rotate(-v, 0, 0);

        //We manage the movement of the player.
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");
        transform.Translate(xAxis * speedPlayer * Time.deltaTime, 0, zAxis * speedPlayer * Time.deltaTime);

        //We manage if the player is moving and stop the sound and animation when they are still.

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

        //Depending on the surface being touched, we will play one audio or another.
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

        //We manage the animations and sound of the hitting mechanic.

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

    // Management of whether the player has lost the game by reaching 0 lives, or if they have won by reaching 3 points,
    // playing the corresponding sounds and videos, and allowing the players to return to the main menu.
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

        //Management to prevent the player from abusing the hitting system to cause damage too frequently.
        if (possibleImpacte == false)
        {
            contador += Time.deltaTime * 1;            
            if (contador >= 3.0f)
            {
                possibleImpacte = true;
                contador = 0f;
            }
        }

        //We manage the AI's lives, and if they reach 0, we respawn them.
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

        //If the player is allowed to perform the attakc and has pressed the attack button('G') and the attack as been executen, then we crecrease the AI's health and perform the animation 
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

        // Input to return to the main menu.
        if (Input.GetKey(KeyCode.Escape))
        {
            managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("MainMenu");
        }
    }

// Method that checks if the cat has collided with various elements of the game,
// such as the dog's object, its base's altar, and water, and applies the corresponding actions to be carried out.
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

    // Method that checks if the cat is touching the grass or the bridge.


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

    //Getterto get gat lives
    public float getVides()
    {
        return vides;
    }

    //Setter to set gat lives
    public void setVides(float i)
    {
        vides -= i * 20;        
    }

// Method to check if the cat has picked up the object or not.
    public bool getTeObjecte()
    {
        return tincOs;
    }

// Method to assign lives to the cat.
public void respawn()
    {
        vides = 100;
    }

// Method to play the impact sound.

    public void playImpacte()
    {
        damageLifeSound.Play();
    }
}
