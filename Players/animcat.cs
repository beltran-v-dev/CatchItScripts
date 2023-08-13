using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class animcat : MonoBehaviourPun, IPunObservable
{
    //inicialització de variables.

    public PhotonView pv;

    public AudioSource walkingGrasSound;

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

    public int force = 200;

    public GameObject imgOs;

    private GameObject refOsGos;

    public GameObject barLife;

    public static bool catMort = false;

    public TextMeshProUGUI points;

    public float pointNumber = 0;

    private bool isDeathZone = false;

    public GameObject imgVideo;

    public GameObject videoPlayer;

    public GameObject panel;

    private bool reproduit = false;

    private bool moviment = false;

    private float timer, timer2;
    private float timeBetweenShots, timeBetweenShots2;
    public AudioClip runGrassClip;
    private bool touchingGrass = false;

    public AudioSource runWood;
    public AudioClip runWoodClip;
    public bool touchingWood = false;

    public AudioSource waterAudio;
    public AudioSource objecteAgafat;
    public AudioSource puntAconseguit;

    public AudioSource winSound;
    public AudioSource loseSound;

    public AudioSource shotingSound;

    private void Start()
    {
        //Si PhtonView és true assignem les variables al jugador

        if (pv.IsMine)
        {
            timeBetweenShots = runGrassClip.length;
            timeBetweenShots2 = runWoodClip.length;

            nameCat.text = PhotonNetwork.NickName;

            posInitial = transform.position;

            animacio = GetComponent<Animator>();

            rb = GetComponent<Rigidbody>();

            observerCam = GameObject.Find("observerCam");

            observerCam.SetActive(false);
            playerCam.SetActive(true);
        }
        else
        {
            //En cas contrari...

            nameCat.text = pv.Owner.NickName;
        }
    }

    private void Update()

    {
        //Amb el mètode InputsPlayer realitzem totes les accions que el player ha de fer, disparar, caminar...

        if (pv.IsMine)
        {
            InputsPlayer();
        }
        else
        {
            //Amb el mètode SmoothMov, fem que el moviment del personatge no sigui brusc per a la resta jugadors que estiguin en linea.

            SmoothMov();
        }
    }

    /**
     * Mètode per a fer que el moviment del jugador i la càmera sigui suau.
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
    * mètode en el que realitzem totes les accions que el player ha de fer, disparar, caminar...
    */

    private void InputsPlayer()
    {
        reproduit = false;

        // Moment del mouse

        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        //Enviem a la xarxa el mètode showLife

        photonView.RPC("showLife", RpcTarget.All);

        //Comprovem si hi som a la DeathZone.

        if (isDeathZone == true)
        {
            vides = vides - 26 * Time.deltaTime;

            if (vides <= 1)
            {
                catMort = true;
            }
        }

        //Si premem la tecla "esc" sortim de la partida desconnectant-nos del servidor i tornar a la pantalla inicial.

        if (Input.GetKey(KeyCode.Escape))
        {
            managerAudio.Instancia.gameObject.GetComponent<AudioSource>().Play();
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainMenu");
        }

        //Si tornem a la posició inicial, tornem a tenir el 100% de la vida

        if (transform.position == posInitial)
        {
            vides = 125f;
        }

        //Si el personatge gat a mort, carreguem el mètode showItemsAfterDeath per a tornar a carregar
        //els items corresponents a la partida.

        //tornem a la posició inicial

        //Recuperem la vida un cop estem a la posició inicial

        if (catMort == true)
        {
            transform.position = posInitial;

            catMort = false;

            vides = 125f;

            photonView.RPC("showItemsAfterDeath", RpcTarget.All);
        }

        //Executem el mètode DamageWeapon

        DamageWeapon();

        //Si tenim 3 punts, en pressionar la tecla "esc" s'atura el vídeo de la victòria i tornem
        //al menú principal, desconnecta-nos del servidor.

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

        //Controlem el moviment de la càmera.

        transform.Rotate(0, h, 0);

        playerCam.transform.Rotate(-v, 0, 0);

        animacio.SetFloat("vertical", Input.GetAxis("Vertical"));

        animacio.SetFloat("horizontal", Input.GetAxis("Horizontal"));

        //Controlem el moviment del jugador.

        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        transform.Translate(xAxis * speed * Time.deltaTime, 0, zAxis * speed * Time.deltaTime);

        //Controlem si el jugador s'està movent i parem el so i l'animació quan està quiet.

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

        //Depenent de la superfície que estigui tocant reproduirem un audio o un altre.

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
     *  OnCollisionEnter per a controlar les diferents col·lisions
     */

    public void OnCollisionEnter(Collision collision)
    {
        //Comprovem a nivell online si estem tocant el terra

        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "floor")
            {
                isGrounded = true;
            }
        }

        // Si detectem l'objecte "osGos", llavors desactivem l'objecte 3d i activem la imatge del gat per a indicar que ha agafat l'os.

        if (collision.gameObject.name == "osGos")
        {
            refOsGos = collision.gameObject;

            collision.gameObject.SetActive(false);

            imgOs.SetActive(true);

            objecteAgafat.Play();
        }

        //Si col·lidim contra l'aigua, tornem a la nostra posició inicial i executem el mètode showItemsAfterDeath.

        if (collision.gameObject.tag == "Water")
        {
            transform.position = posInitial;

            waterAudio.Play();

            photonView.RPC("showItemsAfterDeath", RpcTarget.All);
        }
    }

    /**
     * OnTriggerEnter per detectar les col·lisions de tipus trigger
     */

    public void OnTriggerEnter(Collider other)
    {
        //Si entrem a dintre de l'altar del gat i tenim l'objecte de l'os fem un punt.

        if (other.gameObject.name == "altarCat")
        {
            if (imgOs.activeSelf == true)
            {
                pointNumber++;
            }

            //Tornem a ficar l'objecte i la imatge al seu estat original i reproduïm el so corresponent.

            imgOs.SetActive(false);

            refOsGos.SetActive(true);

            puntAconseguit.Play();

            //Mostrem els punts actuals.

            points.text = pointNumber.ToString();

            //Si tenim tres punts, es reprodueix el vídeo corresponent.

            if (pointNumber == 3)
            {
                panel.SetActive(true);
                imgVideo.SetActive(true);
                videoPlayer.SetActive(true);

                winSound.Play();
            }
        }

        //Detectem quan entrem a la zona NO DeathZone

        if (other.gameObject.tag == "Zona")
        {
            isDeathZone = false;
        }
    }

    /**
     *
     * OnTriggerExit per detectar les col·lisions de tipus trigger
     */

    public void OnTriggerExit(Collider other)
    {
        //  //Detectem quan entrem a la zona DeathZone

        if (other.gameObject.tag == "Zona")
        {
            isDeathZone = true;
        }
    }

    /**
     *
     * OnCollisionExit per detectar les col·lisions de tipus collision
     */

    public void OnCollisionExit(Collision collExist)
    {
        // Si és true isGrounded false.

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
     * Mètode OnPhotonSerializeView, aquest mètode ens serveix per enviar i rebre informació de tots els players de la sala.
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

    /**
     *
     * Mètode OnCollisionStay per a detectar collision.
     *
     */

    public void OnCollisionStay(Collision collision)
    {
        //Comprovacions per a detectar quan és reproduíeu un so o altre.

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

    //Tots els mètodes que hi han a continuació de forma directa o indirecta estan utilitzant la funció PunRPC,
    //aquesta funció és l'encarregada d'indicar que aquests mètodes seran utilitzats a través de la xarxa.

    /**
     *
     * Mètode per al dispar, on controles si el jugador a clickat el botó, si ha detectat l'altre jugador, surt un Line render des de
     * x posició fins a Y posició amb una certa força i distancia.
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

                if (hit.collider.gameObject.tag == "Player1")
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
     * Mètode per executar el LineRender amb un delay.
     *
     */

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;
    }

    /**
     * Mètode per a la gestió del sistema de vida.
     *
     */

    [PunRPC]
    private void showLife()
    {
        barLife.GetComponent<Image>().fillAmount = vides / 100f;

        if (animdog.vides < 1f)
        {
            animdog.vides = 125f;
        }
    }

    /**
    * Mètode per a la gestió del dany.
    *
    */

    [PunRPC]
    private void Damage()
    {
        animdog.vides = animdog.vides - 26f;

        if (animdog.vides < 1f)
        {
            animdog.dogMort = true;

            animdog.vides = 125f;
        }
    }

    /**
    *
    *Mètode que utilitzem al llarg de tot el script, per a que tant l'objecte "refOsGos" com "imgOs" tornen al seu estat inicial.
    */

    [PunRPC]
    private void showItemsAfterDeath()
    {
        imgOs.SetActive(false);
        refOsGos.SetActive(true);
    }
}