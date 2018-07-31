using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathAI : MonoBehaviour {

    private Transform player, target;
    private Vector3[] path;
    private PathFinding pathFinder;
    private GameObject agentCanvas;
    private GameObject agent;
    private bool showAgentCanvas = false;

    // Use this for initialization
    void Start () {
        pathFinder = GetComponent<PathFinding>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectWithTag("Supermarket").transform;
        agentCanvas = GameObject.FindGameObjectWithTag("AgentCanvas");
        agent = GameObject.FindGameObjectWithTag("Agent");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = pathFinder.FindPath(player.position, target.position);
            if(path.Length > 0)
            {
                OnPathFound(path);
            }
            else
            {
                print("ml2ysh");
            }
        }

        if (showAgentCanvas)
        {
            agentCanvas.GetComponent<CanvasGroup>().alpha = 1f;
            agent.transform.localScale = Vector3.one;
        }
        else
        {
            agentCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            agent.transform.localScale = Vector3.zero;
        }
    }

    public void OnPathFound(Vector3[] newPath)
    {
        print("d5alsucs");
        path = newPath;
        Vector3 heading = path[0] - player.position;
        int direction = AngleDir(transform.forward, heading, transform.up);
        showAgentCanvas = true;

        switch (direction)
        {
            case -1: print("Go Left");break;
            case 1: print("Go Right"); break;
            case 0: print("Go Straight"); break;
            default: print("Nothing");break;
        }


    }

    int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
