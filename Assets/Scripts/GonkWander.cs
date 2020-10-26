using UnityEngine;
using UnityEngine.AI;
using PathCreation;
using PathCreation.Examples;

[RequireComponent(typeof(NavMeshAgent))]

public class GonkWander : MonoBehaviour
{

    NavMeshAgent  nm;
    Animator anim;
    public AudioClip ouch;
    public Transform Target;
    public int curWaypoint;
    public float speed, stop_distance;
    public float PauseTimer;
    [SerializeField]
    private float curTimer;
    // Start is called before the first frame update
    void Start()
    {
        //nm = GetComponent<NavMeshAgent>();

        //GetComponent<PathFollower>().pathCreator = GameObject.Find("Gonk Path").GetComponent<PathCreator>();

        //Target = WayPoints[curWaypoint].GetComponent<Transform>();
        //curTimer = PauseTimer;

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
