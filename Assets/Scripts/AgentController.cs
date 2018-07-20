using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    private Animator anim;
    private NavMeshAgent agent;
    private int size;
    private bool finished = false;


    // Use this for initialization
    void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
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
        size = nodes.Count;

        if (size == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.SetDestination(nodes[currentNode].position);
        currentNode += 1;

        if (currentNode == size)
        {
            //Debug.Log(destPoint);
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
