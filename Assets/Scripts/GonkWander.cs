using UnityEngine;
using UnityEngine.AI;
using PathCreation;
using PathCreation.Examples;

[RequireComponent(typeof(NavMeshAgent))]

public class GonkWander : MonoBehaviour
{
    Animator anim;
    public AudioClip ouch;

    void Start()
    {

        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = ouch;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)  //Plays Sound Whenever collision detected
    {
        Debug.Log("GonkWander: Someone just hit me!");
        GetComponent<AudioSource>().Play();
    }
}
