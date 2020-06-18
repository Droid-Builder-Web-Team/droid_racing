using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    private static int numCheckpoints = 4; 
    public bool[] checkPoints = new bool[numCheckpoints];
        
    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;
        
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    
    private WebCalls webCalls;
    
    // Start is called before the first frame update
    void Start()
    {
      
        webCalls = (WebCalls)GameObject.Find("WebCalls").GetComponent(typeof(WebCalls));
        Debug.Log("Webcalls Object: {0}" + webCalls);
        if (photonView.IsMine) {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        
        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
        
        
        characterController = GetComponent<CharacterController>();
        if (PlayerUiPrefab != null)
        {
            GameObject _uiGo =  Instantiate(PlayerUiPrefab);
            _uiGo.SendMessage ("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }               
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed, 0);
        characterController.SimpleMove(forward * currentSpeed);
        
        elapsedTime = Time.time - startTime;
        
      }
    }
    
    
    void Awake() 
    {
        if (!photonView.IsMine)
        {
            enabled = false;
        }
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        PlayerMove.LocalPlayerInstance = this.gameObject;

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);  
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit a collider");
        if (!photonView.IsMine)
        {
            Debug.Log("It wasn't me");
            return;
        }

        if (other.tag == "Checkpoint")
        {
            Debug.Log("Checkpoint crossed", other);
            int checkPointNumber = other.GetComponent<Checkpoint>().checkPointNumber;
            checkPoints[checkPointNumber] = true;
            return;        Vector3 targetPosition;
        }
        if (other.tag == "Finish") 
        {
            Debug.Log("Crossed the finish line");
            bool lapDone = true;
            bool lapStart = true;
            // Check if we need to start the timer...
            for(int i = 0; i < numCheckpoints; i++)
            {
                if (checkPoints[i]) 
                {
                    lapStart = false;
                    Debug.Log("Don't restart timer");
                }
            }
            for(int i = 0; i < numCheckpoints; i++)
            {
                if (!checkPoints[i]) 
                {
                    lapDone = false;
                    Debug.Log("You missed a checkpoint");
                }
            }
            if (lapDone) 
            {
                Debug.Log("Yay, you crossed the finsh line!");
                laps++;
                lastTime = Time.time - startTime;
                if (bestTime == 0 || bestTime > lastTime)
                    bestTime = lastTime;
                for(int i = 0; i < numCheckpoints; i++)
                    checkPoints[i] = false;
                lapStart = true;
                webCalls.UploadLap("email", PhotonNetwork.NickName, lastTime, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.Name);
                //webCalls.TestLap();
            }
            if (lapStart) 
            {
                Debug.Log("Starting a lap...");
                startTime = Time.time;
            }
        }
        
    }

}

}