using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform agent;
    public Animator agentAnim;
    public Transform player;
    public AgentController controller;
    private bool once = false;
    bool x = true;

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

    IEnumerator Delay()
    {
        controller.getAgent().isStopped = true;
        agentAnim.Play("M_idle1");
        yield return new WaitForSeconds(2.0f);
        agentAnim.Play("M_turnR90");
        yield return new WaitForSeconds(2.0f);
        agentAnim.Play("M_clap");
        yield return new WaitForSeconds(2.0f);
    }

    private void Update()
    {
        if (!once)
        {
            if (controller != null)
            {
                if (controller.isFinished())
                {
                    if (player != null)
                    {
                        //StartCoroutine(Delay());
                        setTarget(player);
                        once = true;
                    }

                    agent.transform.localScale = new Vector3(0, 0, 0);
                }
            }
            else
            {
                Debug.LogError("Controller Null");
            }
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
