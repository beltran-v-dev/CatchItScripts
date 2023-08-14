using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class animdog : MonoBehaviourPun, IPunObservable
{
    //Variables delcaration(

    public PhotonView pv;

    public GameObject positionShot;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private RaycastHit hit;

    public GameObject playerCam;
    public float weaponRange = 50f;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    private float jumpForce = 400f;

    private bool isGrounded;

    public static float vides = 125f;

    public GameObject posicioInicialPlayer;

    public GameObject observerCam;

    public Camera playCamShotRef;

    public float nextFire;

    private Vector3 smoothMove;

    public float horizontalSpeed;
    public float verticalSpeed;

    private float speed = 10f;

    public float speedRun = 15f;

    public float fireRate = 2f;

    private Rigidbody rb;

    private float h;
    private float v;

    private Quaternion realRotation;

    public Animator animacio;

    public TextMeshProUGUI nameCat;

    public LineRenderer laserLine;

    public GameObject gun;

    public Vector3 posInitial;

    private Vector3 rayOrigin;

    private Vector3 posRay = new Vector3();

    public GameObject projectile;
    public int force = 100;

    public GameObject imgGat;

    private GameObject refRaspaPeixGat;

    public GameObject barLife;

    public static bool dogMort = false;

    public TextMeshProUGUI points;

    public float pointNumber = 0;

    private bool isDeathZone = false;

    public GameObject imgVideo;

    public GameObject videoPlayer;

    public GameObject panel;

    private bool moviment = false;

    private float timer, timer2, timerWin, timerLose;
    private float timeBetweenShots, timeBetweenShots2, timeBetweenShotsWin, timeBetweenShotsLose;
    public AudioClip runGrassClip;
    private bool touchingGrass = false;

    public AudioSource runWood;
    public AudioClip runWoodClip;
    public bool touchingWood = false;

    public AudioSource waterAudio;
    public AudioSource walkingGrasSound;

    public AudioSource objecteAgafat;
    public AudioSource puntAconseguit;

    private bool reproduit = false;

    public AudioSource winSound;
    public AudioSource loseSound;

    public AudioClip soundFinalLose;
    public AudioClip soundFinalWin;

    public AudioSource shotingSound;

    private void Start()
    {
        // If PhotonView is true, we assign the variables to the player.

        if (pv.IsMine)
        {
            timeBetweenShots = runGrassClip.length;
            timeBetweenShots2 = runWoodClip.length;
            timeBetweenShotsWin = soundFinalWin.length;
            timeBetweenShotsLose = soundFinalLose.length;

            nameCat.text = PhotonNetwork.NickName;

            posInitial = transform.position;

            animacio = GetComponent<Animator>();
            Debug.Log("Vida: " + vides);

            rb = GetComponent<Rigidbody>();

            observerCam = GameObject.Find("observerCam");

            observerCam.SetActive(false);
            playerCam.SetActive(true);
        }
        else
        {
            //Otherwise

            nameCat.text = pv.Owner.NickName;
        }
    }

    private void Update()

    {
        //With the InputsPlayer method, we perform all the actions that the player needs to do, such as shooting, walking...

        if (pv.IsMine)
        {
            InputsPlayer();
        }
        else
        {
            //With the SmoothMov method, we ensure that the character's movement is not abrupt for the other players who are online.

            SmoothMov();
        }
    }

    /**
     * Method to make the movement of the player and the camera smooth.
     */

    private void SmoothMov()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);

        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(0, h, 0);

        playerCam.transform.Rotate(-v, 0, 0);

        realRotation = Quaternion.Lerp(transform.rotation, realRotation, 1f);
    }

    /**
    * Method in which we perform all the actions that the player needs to do, such as shooting, walking...
    */

    private void InputsPlayer()
    {
        // Mouse movement

        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        //We check if we are in the DeathZone.

        if (isDeathZone == true)
        {
            vides = vides - 26 * Time.deltaTime;

            if (vides <= 1)
            {
                dogMort = true;
            }
        }

        //We send the showLife method to the network.

        photonView.RPC("showLife", RpcTarget.All);

        //We execute the DamageWeapon method.

        DamageWeapon();

        //If we press the "esc" key, we exit the game by disconnecting from the server and returning to the main screen.

        if (Input.GetKey(KeyCode.Escape))
        {
            managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Play();
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
        }

        //If we return to the initial position, we regain 100% of our health.

        if (transform.position == posInitial)
        {
            vides = 125f;
        }

     
        // If the dog character is dead, we load the showItemsAfterDeath method to reload
        // the items corresponding to the game.

        //We return to the initial position.

        //We recover the health once we are in the initial position.

        if (dogMort == true)
        {
            transform.position = posInitial;

            dogMort = false;

            vides = 125f;

            photonView.RPC("showItemsAfterDeath", RpcTarget.All);
        }

    // If we have 3 points, when we press the "esc" key, the victory video stops, and we return
    // to the main menu, disconnecting from the server.

        if (pointNumber == 3)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                imgVideo.SetActive(false);
                videoPlayer.SetActive(false);
                panel.SetActive(false);

                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("MainMenu");
            }
        }

 
    // We manage the movement of the camera.

        transform.Rotate(0, h, 0);

        playerCam.transform.Rotate(-v, 0, 0);

        animacio.SetFloat("vertical", Input.GetAxis("Vertical"));

        animacio.SetFloat("horizontal", Input.GetAxis("Horizontal"));

        // We manage the movement of the player.

        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        transform.Translate(xAxis * speed * Time.deltaTime, 0, zAxis * speed * Time.deltaTime);

       // We manage if the player is moving and stop the sound and animation when they are still.

        if (zAxis != 0 || xAxis != 0)
        {
            animacio.SetBool("vertical_b", true);

            moviment = true;
        }
        else
        {
            animacio.SetBool("vertical_b", false);

            moviment = false;
            walkingGrasSound.Stop();
            runWood.Stop();
        }

     // Depending on the surface the character is on, we will play one audio or another.

        if (touchingGrass)
        {
            runWood.Stop();

            timer += Time.deltaTime;

            if (timer > timeBetweenShots)
            {
                walkingGrasSound.PlayOneShot(runGrassClip);

                timer = 0;
            }
        }
        else
        {
            walkingGrasSound.Stop();

            timer2 += Time.deltaTime;
            if (timer2 >= timeBetweenShots2)
            {
                runWood.PlayOneShot(runWoodClip);
                timer2 = 0;
            }
        }
    }

    /**
     *  OnCollisionEnter to control the different collisions.
     */

    public void OnCollisionEnter(Collision collision)
    {
        //we check if the character is touching the ground when is playing online 

        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "floor")
            {
                isGrounded = true;
            }

            //If we collide with water, we return to our initial position and execute the showItemsAfterDeath method.

            if (collision.gameObject.tag == "Water")
            {
                transform.position = posInitial;

                waterAudio.Play();

                photonView.RPC("showItemsAfterDeath", RpcTarget.All);
            }
        }

        //We apply damage to player2.

        if (collision.gameObject.tag == "Player2")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("Damage", RpcTarget.All);
            }
        }

      // If we detect the "raspaPeix," then we deactivate the 3D object and activate the cat image to indicate that it has picked up the fish bone.

        if (collision.gameObject.name == "raspaPeix")
        {
            refRaspaPeixGat = collision.gameObject;

            collision.gameObject.SetActive(false);

            imgGat.SetActive(true);

            objecteAgafat.Play();
        }
    }

    /**
     * OnTriggerEnter to detect trigger-type collisions
     */

    public void OnTriggerEnter(Collider other)
    {
  // If we enter inside the dog's altar and have the fish bone object, we score a point

        if (other.gameObject.name == "altarDog")
        {
            if (imgGat.activeSelf)
            {
                pointNumber++;
                puntAconseguit.Play();
            }
            //Si tenim tres punts reproduïm  el vídeo

            if (pointNumber == 3)
            {
                panel.SetActive(true);
                imgVideo.SetActive(true);
                videoPlayer.SetActive(true);

                winSound.Play();
            }

            //We activate the corresponding object and image.

            imgGat.SetActive(false);
            refRaspaPeixGat.SetActive(true);

            //We display the points scored.

            points.text = pointNumber.ToString();
        }

        //We detect when we enter the NO DeathZone area.

        if (other.gameObject.tag == "Zona")
        {
            isDeathZone = false;
        }
    }

    /**
     *
         OnTriggerExit to detect trigger-type collisions when exiting.
     */

    public void OnTriggerExit(Collider other)
    {
        // We detect when we enter the DeathZone area.

        if (other.gameObject.tag == "Zona")
        {
            if (other.gameObject.tag == "Zona")
            {
                isDeathZone = true;
            }
        }
    }

    /**
   *
   * OnCollisionExit to detect collision-type interactions when exiting.
   */

    public void OnCollisionExit(Collision collExist)
    {
       
        //If it is true, isGrounded becomes false.

        if (photonView.IsMine)
        {
            if (collExist.gameObject.tag == "floor")
            {
                isGrounded = false;
            }
        }
    }

    /**
    *
    * Method OnCollisionStay to detect collisions.
    *
    */

    public void OnCollisionStay(Collision collision)
    {
        //Checks to detect when one sound or another is being played.

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

    /**
     *
     * Method OnPhotonSerializeView, this method is used to send and receive information from all players in the room.
     */

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(vides);
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            vides = (float)stream.ReceiveNext();
        }
    }

    // All the methods that follow, either directly or indirectly, are using the PunRPC function,
    // this function is responsible for indicating that these methods will be used over the network.

    /**
     *
        Method for shooting, where you control if the player has clicked the button, if it has detected the other player, a Line render comes out from
        x position to Y position with a certain force and distance.
*/
     */
    [PunRPC]
    public void DamageWeapon()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            shotingSound.Play();

            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            rayOrigin = playCamShotRef.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            Ray ray = playCamShotRef.ScreenPointToRay(Input.mousePosition);

            laserLine.SetPosition(0, gun.transform.position);

            if (Physics.Raycast(rayOrigin, playCamShotRef.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                if (hit.collider.gameObject.tag == "Player2")
                {
                    if (photonView.IsMine)
                    {
                        photonView.RPC("Damage", RpcTarget.All);
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (playCamShotRef.transform.forward * weaponRange));
            }
        }
    }

    /**
     * Method to execute the LineRender with a delay.
     *
     */

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }

    /**
    * Method for managing the health system.
    *
    */

    [PunRPC]
    private void showLife()
    {
        barLife.GetComponent<Image>().fillAmount = vides / 125f;

        if (animcat.vides < 1f)
        {
            animcat.vides = 125f;
        }
    }

    /**
    * Method for managing damage.
    */

    [PunRPC]
    private void Damage()
    {
        animcat.vides = animcat.vides - 26f;

        if (animcat.vides < 1f)
        {
            animcat.catMort = true;

            animcat.vides = 125f;
        }
    }

    /**
    *Method used throughout the script to reset both the "refOsGos" object and "imgOs" image to their initial state.
    */

    [PunRPC]
    private void showItemsAfterDeath()
    {
        imgGat.SetActive(false);
        refRaspaPeixGat.SetActive(true);
    }
}
