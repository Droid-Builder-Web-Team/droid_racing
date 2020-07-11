using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class GonkWander : MonoBehaviour
{
  
    NavMeshAgent  nm;
    Animator anim;
    public AudioClip ouch;
    public Transform Target;
    public GameObject[] WayPoints;
    public int curWaypoint;
    public float speed, stop_distance;
    public float PauseTimer;
    [SerializeField]
    private float curTimer;
    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        
        WayPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        Target = WayPoints[curWaypoint].GetComponent<Transform>();
        curTimer = PauseTimer;
                
        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = ouch;
    }

    // Update is called once per frame
    void Update()
    {
        nm.acceleration = speed;
        nm.stoppingDistance = stop_distance;
        
        float distance = Vector3.Distance(transform.position, Target.position);
        if (distance > stop_distance && WayPoints.Length > 0)
        {
            Target = WayPoints[curWaypoint].GetComponent<Transform>();
        }
        else if (distance <= stop_distance && WayPoints.Length > 0)
        {
            if (curTimer > 0)
            {
                curTimer -= 0.01f;
            }
            if (curTimer <= 0)
            {
                curWaypoint++;
                if (curWaypoint >= WayPoints.Length)
                {
                    curWaypoint = 0;
                }
                Target = WayPoints[curWaypoint].GetComponent<Transform>();
                curTimer = PauseTimer;
            }
        }
        nm.SetDestination(Target.position);
    }
    
    void OnTriggerEnter(Collider other)  //Plays Sound Whenever collision detected
    {
        Debug.Log("GonkWander: Someone just hit me!");
        GetComponent<AudioSource>().Play();
    }
}
