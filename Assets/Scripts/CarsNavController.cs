using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarsNavController : MonoBehaviour {

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    public int size;
    public bool reverse = false;
    private Animator anim;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // anim = GetComponent<Animator>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        // anim.Play("M_walk");

        size = points.Length;

        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (size == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        //if (reverse)
        //{
        //    destPoint -= 1;
        //}
        //else
        //{
        //    destPoint += 1;
        //}

        //if(destPoint == size)
        //{
        //    reverse = true;
        //} else if (destPoint == 0)
        //{
        //    reverse = false;
        //}

    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 10))
        {
            if (hit.collider.gameObject.CompareTag("TLS"))
            {
                TLSController controller = hit.collider.gameObject.GetComponent<TLSController>();
                int trafficState = controller.state; 

                if(trafficState == 1 || trafficState == 2)
                {
                    hit.collider.enabled = true;
                    print("State: " + trafficState);
                    agent.isStopped = true;
                } else if (trafficState == 3)
                {
                    hit.collider.enabled = false;
                    print("State: " + trafficState);
                    agent.isStopped = false;

                }
            }
        } else if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
