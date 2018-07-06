using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{

    public Transform[] points;
    private Animator anim;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private int size;
    private bool finished = false;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        anim.Play("M_walk");
        GoToSupermarket();
    }

    public NavMeshAgent getAgent()
    {
        return this.agent;
    }

    public void GoToSupermarket()
    {
        size = points.Length;

        if (size == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.SetDestination(points[destPoint].position);

        destPoint += 1;

        if (destPoint == size)
        {
            Debug.Log(destPoint);
            //agent.isStopped = true;
            finished = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !finished)
            GoToSupermarket();
    }

    public bool isFinished()
    {
        return finished;
    }
}
