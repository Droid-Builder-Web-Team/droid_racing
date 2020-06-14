using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController characterController; 
    public float acceleration = 0.2f;
    public float topSpeed = 3f;
    public float turnSpeed = 2f;
    public float dragFactor = 2f;

    private float gravity = 20.0f;
    public float currentSpeed = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
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
