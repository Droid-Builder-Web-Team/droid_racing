using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace uk.droidbuilders.droid_racing
{

public class PlayerMove : MonoBehaviourPun
{
    public CharacterController characterController; 
    public float acceleration = 0.2f;
    public float topSpeed = 3f;
    public float turnSpeed = 2f;
    public float dragFactor = 2f;

    private float gravity = 20.0f;
    public float currentSpeed = 0f;
    
    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;
        
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    
    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
      if (photonView.IsMine) 
      {
        if (characterController.isGrounded) {
            currentSpeed += acceleration*Input.GetAxis ("Vertical");
            currentSpeed = Mathf.Clamp(currentSpeed,-topSpeed,topSpeed); // Clamps curSpeed
            if(Input.GetAxis("Vertical") == 0) {
                if(currentSpeed > 0) currentSpeed -= dragFactor; 
                if(currentSpeed < 0) currentSpeed += dragFactor; 
            }
            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed, 0);
            
            

        }  
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        characterController.SimpleMove(forward * currentSpeed);
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
}

}