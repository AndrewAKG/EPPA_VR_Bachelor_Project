using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShoppersAI : MonoBehaviour {

    public Transform path;
    public GameObject shopper;
    private List<Transform> nodes;
    private NavMeshAgent agent;
    private int currentNode = 0;
    private bool finished;
    private Animator anim;
    private Vector3 originalPos;

	// Use this for initialization
	void Start () {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true; 
        agent.updatePosition = true;
        agent.autoBraking = false;

        originalPos = transform.position;
        GoToNextNode();
    }

    private void GoToNextNode()
    {
        agent.destination = nodes[currentNode].position;

        currentNode += 1;

        if (currentNode == nodes.Count)
        {
            Debug.Log(currentNode);
            //agent.isStopped = true;
            finished = true;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !finished)
            GoToNextNode();

        if (finished)
        {
            print("Da5al");
            Destroy(gameObject, .5f);
            Instantiate(shopper, originalPos, transform.rotation);
            currentNode = 0;
            finished = false;
        }
    }

    public bool isFinished()
    {
        return finished;
    }
}
