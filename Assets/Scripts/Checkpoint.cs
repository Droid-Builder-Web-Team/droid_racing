using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int checkPointNumber;
    public AudioClip check;
    void Start()
    {
        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = check;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)  //Plays Sound Whenever collision detected
    {
        Debug.Log("Checkpoint: Someone went through me!");
        GetComponent<AudioSource> ().Play ();
    }
}
