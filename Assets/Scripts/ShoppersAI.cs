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
    private Vector3 sensorOrigin;
    private Vector3 sensorOffset = new Vector3(0f, 0.9f, 0.3f);

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

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true; 
        agent.updatePosition = true;
        agent.autoBraking = false;

        print(GetComponent<Collider>().bounds.size);

        originalPos = transform.position;
        anim.SetBool("walking", true);
       // GoToNextNode();
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

        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        sensorOrigin = transform.position;
        sensorOrigin += transform.forward * sensorOffset.z;
        sensorOrigin += transform.up * sensorOffset.y;

        if (Physics.Raycast(sensorOrigin, fwd, out hit, 2f))
        {
            if (hit.collider.gameObject.CompareTag("AIShopper") || hit.collider.gameObject.CompareTag("AICart"))
            {
                Debug.DrawLine(sensorOrigin, hit.point);
                print(hit.collider.gameObject);
                //anim.Play("M_idle1");
                anim.SetBool("walking", false);
                agent.isStopped = true;
            }     
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f && !finished) {
            agent.isStopped = false;
            anim.SetBool("walking", true);
            //anim.Play("M_walk");
            GoToNextNode();
        }

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
