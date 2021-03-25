using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace uk.droidbuilders.droid_racing
{

public class PlayerMove : MonoBehaviourPun
{
    public CharacterController characterController;
    public float acceleration = 15f;
    public float topSpeed = 30f;
    public float turnSpeed = 3f;
    public float dragFactor = 60f;

    public float currentSpeed = 0f;
    public int laps = 0;
    private float startTime;
    public float elapsedTime;
    public float lastTime;
    public float bestTime;
    private bool hasStarted = false;
    public Color color;

    private static int numCheckpoints = 4;
    public bool[] checkPoints = new bool[numCheckpoints];

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public Material PlayerMaterial;

    private WebCalls webCalls;

    // Start is called before the first frame update
    void Start()
    {

        webCalls = (WebCalls)GameObject.Find("WebCalls").GetComponent(typeof(WebCalls));
        Debug.Log("PlayerMove: Webcalls Object: {0}" + webCalls);
        if (photonView.IsMine) {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            //ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            //hashtable.Add("email", PlayerPrefs.GetString("PlayerEmail"));
            //PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

            Color randomcolor = Random.ColorHSV();
            color = new Color(
                PlayerPrefs.GetFloat("rValue", randomcolor.r),
                PlayerPrefs.GetFloat("gValue", randomcolor.g),
                PlayerPrefs.GetFloat("bValue", randomcolor.b)
            );

            PlayerPrefs.SetFloat("rValue", color.r);
            PlayerPrefs.SetFloat("gValue", color.g);
            PlayerPrefs.SetFloat("bValue", color.b);

            this.photonView.RPC("RPC_SendColor", RpcTarget.AllBuffered, new Vector3(color.r, color.g, color.b));

            if (_cameraWork != null)
            {
                Debug.Log("PlayerMove: Setting CameraWork to Follow");
                _cameraWork.OnStartFollowing();
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }


            characterController = GetComponent<CharacterController>();
            if (PlayerUiPrefab != null)
            {
                Debug.Log("PlayerMove: Instantiating PlayerUI");
                GameObject _uiGo =  Instantiate(PlayerUiPrefab);
                _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("rValue", color.r);
            hashtable.Add("gValue", color.g);
            hashtable.Add("bValue", color.b);
            hashtable.Add("laps", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        }
    }

    // Update is called once per frame

    void Update()
    {
      if (photonView.IsMine)
      {
        if (characterController.isGrounded) {

            if ((Input.GetAxis("Vertical") < 0) && (currentSpeed > -topSpeed))
            {
                currentSpeed = currentSpeed + (Input.GetAxis("Vertical")*(acceleration * Time.deltaTime));
            } else if ((Input.GetAxis("Vertical")  > 0) && (currentSpeed < topSpeed))
            {
                currentSpeed = currentSpeed + acceleration * Time.deltaTime;
            } else
            {
                if(currentSpeed > dragFactor * Time.deltaTime)
                    currentSpeed = currentSpeed - dragFactor * Time.deltaTime;
                else if(currentSpeed < -dragFactor * Time.deltaTime)
                    currentSpeed = currentSpeed + dragFactor * Time.deltaTime;
                else
                    currentSpeed = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            toggleSound();
        }
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        characterController.SimpleMove(forward * currentSpeed);
        if (hasStarted)
        {
            elapsedTime = Time.time - startTime;
        }
      }
    }


    void Awake()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        }
        Debug.Log("PlayerMove: Awake!");

        if ( photonView.IsMine)
        {
            PlayerMove.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        Debug.Log("PlayerMove: OnSceneLoaded");
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
/*
    void OnLevelWasLoaded(int level)
    {
        Debug.Log("PlayerMove: OnLevelWasLoaded");
        this.CalledOnLevelWasLoaded(level);
    }
*/

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        Debug.Log("PlayerMove: CalledOnLevelWasLoaded");
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    private void OnDisable()
    {
        Debug.Log("PlayerMove: OnDisable");
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PlayerMove: Hit a collider");
        if (!photonView.IsMine)
        {
            Debug.Log("PlayerMove: It wasn't me");
            return;
        }

        if (other.tag == "Checkpoint")
        {
            Debug.Log("PlayerMove: Checkpoint crossed", other);
            int checkPointNumber = other.GetComponent<Checkpoint>().checkPointNumber;
            checkPoints[checkPointNumber] = true;
            return;
        }
        if (other.tag == "Finish")
        {
            Debug.Log("PlayerMove: Crossed the finish line");
            hasStarted = true;
            bool lapDone = true;
            bool lapStart = true;
            // Check if we need to start the timer...
            for(int i = 0; i < numCheckpoints; i++)
            {
                if (checkPoints[i])
                {
                    lapStart = false;
                    Debug.Log("PlayerMove: Don't restart timer");
                }
            }
            for(int i = 0; i < numCheckpoints; i++)
            {
                if (!checkPoints[i])
                {
                    lapDone = false;
                    Debug.Log("PlayerMove: You missed a checkpoint");
                }
            }
            if (lapDone)
            {
                Debug.Log("PlayerMove: Yay, you crossed the finsh line!");
                laps++;
                lastTime = Time.time - startTime;
                if (bestTime == 0 || bestTime > lastTime)
                    bestTime = lastTime;
                for(int i = 0; i < numCheckpoints; i++)
                    checkPoints[i] = false;
                lapStart = true;
                StartCoroutine(webCalls.UploadLap(PlayerPrefs.GetString("PlayerEmail"), PhotonNetwork.NickName, lastTime, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.Name, (int)PhotonNetwork.CurrentRoom.CustomProperties["map"]));
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add("laps", laps);
                hashtable.Add("best", bestTime);
                hashtable.Add("last", lastTime);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            }
            if (lapStart)
            {
                Debug.Log("PlayerMove: Starting a lap...");
                startTime = Time.time;
            }
        }
        Debug.Log("PlayerMove: Laps - " + laps + " Best - " + bestTime + " Last - " + lastTime);
    }

    [PunRPC]
    private void RPC_SendColor(Vector3 color)
    {
        Debug.Log("PlayerMove: RPC_SendColor called");
        Color playercolor = new Color(color.x, color.y, color.z);
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", playercolor);
    }

    public void toggleSound() {
        AudioListener _listener = gameObject.GetComponent<AudioListener>();
        if (_listener.enabled)
        {
            _listener.enabled = false;
        }
        else
        {
            _listener.enabled = true;
        }
    }

}

}
