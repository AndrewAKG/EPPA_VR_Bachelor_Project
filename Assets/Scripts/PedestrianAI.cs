using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianAI : MonoBehaviour {

    public Transform path;
    private List<Transform> points;
    private int currentNode = 0;
    private Animator anim;
    private NavMeshAgent agent;
    private int size;

    // Use this for initialization
    void Start () {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        points = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                points.Add(pathTransforms[i]);
            }
        }
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        size = points.Count;
        GotoNextPoint();
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (size == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[currentNode].position;


        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        currentNode = (currentNode + 1) % points.Count;
    }

    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
