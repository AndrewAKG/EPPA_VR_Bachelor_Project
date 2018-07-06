using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform agent;
    public Transform player;
    public AgentController controller;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    private void Start()
    {
        if (agent != null)
            setTarget(agent);
    }

    private void Update()
    {
        if (controller != null)
        {
            if (controller.isFinished())
            {
                if (player != null)
                    setTarget(player);

                agent.transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            Debug.LogError("Controller Null");
        }
    }

    private void LateUpdate()
    { 
        Refresh();
    }

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}
