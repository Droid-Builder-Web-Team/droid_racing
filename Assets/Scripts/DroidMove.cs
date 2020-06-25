
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidMove : MonoBehaviour
{

    public WheelCollider leftTrackA;
    //public WheelCollider leftTrackB;
    public WheelCollider rightTrackA;
    //public WheelCollider rightTrackB;

    private float rawLeft;
    private float rawRight;

    public float MaxValue = 500;
    public float MinValue = -500;

    private float RawLeft;
    private float RawRight;

    public float ValLeft;
    public float ValRight;
    
    public float brakeTorque;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the Cartesian input (joystick/keyboard)

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // If you want to send the raw values to and
        // perform calculation on the robot you can
        // use the following line. JV tells the receiving
        // code that the data is just basic Joysick values.
        // string data = "V," + x + "," + y;
        // Otherwise you have two additional options,
        // the sending of Raw or Value data
        // Use the x,y input to derive the wheel speeds

        CalculateTankDrive(x, y);


        leftTrackA.motorTorque = ValLeft;
        rightTrackA.motorTorque = ValRight;
        if (ValLeft == 0) {
          leftTrackA.brakeTorque = brakeTorque;
        } else {
          leftTrackA.brakeTorque = 0;
        }
        if (ValRight == 0) {
          rightTrackA.brakeTorque = brakeTorque;
        } else {
          rightTrackA.brakeTorque = 0;
        }
        //leftTrackB.motorTorque = ValLeft;
        //rightTrackB.motorTorque = ValRight;
    }

    public void CalculateTankDrive(float x, float y)
    {
        // first Compute the angle in deg
        // First hypotenuse
        var z = Mathf.Sqrt(x * x + y * y);
        // angle in radians
        var rad = Mathf.Acos(Mathf.Abs(x) / z);

        if (float.IsNaN(rad))
            rad = 0;
        // and in degrees
        var angle = rad * 180 / Mathf.PI;

        // Now angle indicates the measure of turn
        // Along a straight line, with an angle o, the turn co-efficient is same
        // this applies for angles between 0-90, with angle 0 the co-eff is -1
        // with angle 45, the co-efficient is 0 and with angle 90, it is 1

        var tcoeff = -1 + (angle / 90) * 2;
        var turn = tcoeff * Mathf.Abs(Mathf.Abs(y) - Mathf.Abs(x));
        turn = Mathf.Round(turn * 100) / 100;

        // And max of y or x is the movement

        var move = Mathf.Max(Mathf.Abs(y), Mathf.Abs(x));

        // First and third quadrant

        if ((x >= 0 && y >= 0) || (x < 0 && y < 0))
        {
            rawLeft = move;
            rawRight = turn;
        }
        else
        {
            rawRight = move;
            rawLeft = turn;
        }

        // Reverse polarity

        if (y < 0)
        {
            rawLeft = 0 - rawLeft;
            rawRight = 0 - rawRight;
        }

        RawLeft = rawLeft;
        RawRight = rawRight;

        ValLeft = Remap(rawLeft, MinValue, MaxValue);
        ValRight = Remap(rawRight, MinValue, MaxValue);
    }

    public float Remap(float value, float from2, float to2)
    {
        return (value + 1) / (2) * (to2 - from2) + from2;
    }



}