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
    private bool pedestrianStateChanged = false;
    private int size;
    private Vector3 sensorOrigin;
    private Vector3 sensorOffset = new Vector3(0f, 0.9f, 0.1f);
    private float frontSensorAngle = 30f;

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
        anim.SetBool("walking", true);
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
        currentNode = Random.Range(0, points.Count);
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        sensorOrigin = transform.position;
        sensorOrigin += transform.forward * sensorOffset.z;
        sensorOrigin += transform.up * sensorOffset.y;

        if (Physics.Raycast(sensorOrigin, fwd, out hit, 2f))
        {
            if (hit.collider.gameObject.CompareTag("TLSPC"))
            {
                GameObject collisionObject = hit.collider.gameObject;
                GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
                PedestrianLight light = collisionObjectParent.GetComponent<PedestrianLight>();
                int trafficState = light.getState();

                if (trafficState == 1)
                {
                    Debug.DrawLine(sensorOrigin, hit.point);
                    anim.SetBool("walking", false);
                    agent.isStopped = true;
                    pedestrianStateChanged = true;
                }
            }

            if (hit.collider.gameObject.CompareTag("AICar"))
            {
                Debug.DrawLine(sensorOrigin, hit.point);
                anim.SetBool("walking", false);
                agent.isStopped = true;
                pedestrianStateChanged = true;
            }
        }

        //if (Physics.Raycast(sensorOrigin, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, 2f))
        //{
        //    if (hit.collider.gameObject.CompareTag("TLSPC"))
        //    {
        //        GameObject collisionObject = hit.collider.gameObject;
        //        GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
        //        PedestrianLight light = collisionObjectParent.GetComponent<PedestrianLight>();
        //        int trafficState = light.getState();

        //        if (trafficState == 1)
        //        {
        //            Debug.DrawLine(sensorOrigin, hit.point);
        //            anim.SetBool("walking", false);
        //            agent.isStopped = true;
        //            pedestrianStateChanged = true;
        //        }
        //    }
        //}

        //if (Physics.Raycast(sensorOrigin, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, 2f))
        //{
        //    if (hit.collider.gameObject.CompareTag("TLSPC"))
        //    {
        //        GameObject collisionObject = hit.collider.gameObject;
        //        GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
        //        PedestrianLight light = collisionObjectParent.GetComponent<PedestrianLight>();
        //        int trafficState = light.getState();

        //        if (trafficState == 1)
        //        {
        //            Debug.DrawLine(sensorOrigin, hit.point);
        //            anim.SetBool("walking", false);
        //            agent.isStopped = true;
        //            pedestrianStateChanged = true;
        //        }
        //    }
        //}

        if (!pedestrianStateChanged && agent.isStopped)
        {
            agent.isStopped = false;
            anim.SetBool("walking", true);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !pedestrianStateChanged)
        {
            GotoNextPoint();
        }

        pedestrianStateChanged = false;
    }
}
