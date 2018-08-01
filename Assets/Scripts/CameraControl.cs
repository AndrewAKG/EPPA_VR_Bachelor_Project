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
    private AudioSource startsound;

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
        StartCoroutine(CameraToAgent());
    }

    IEnumerator CameraToAgent()
    {
        if (agent != null)
            setTarget(agent);

        startsound.Play();
        yield return new WaitForSeconds(8.5f);
        agent.GetComponent<AgentController>().enabled = true;
    }

    void Delay()
    {
        controller.getAgent().isStopped = true;
        //this.lookAt = true;
        //agentAnim.SetBool("Finished", true);
        //yield return new WaitForSeconds(10);
        once = true;
        //this.lookAt = false;
        setTarget(player);
        agent.transform.localScale = Vector3.zero;
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
                        Delay();    
                    }
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
