using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPathAI : MonoBehaviour {

    Transform player, target;
    Vector3[] path;
    PathFinding pathFinder;

    // Use this for initialization
    void Start () {
        pathFinder = GetComponent<PathFinding>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectWithTag("Supermarket").transform;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("d5al");
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
    }

    public void OnPathFound(Vector3[] newPath)
    {
        print("d5alsucs");
        path = newPath;
        Vector3 heading = path[0] - player.position;
        int direction = AngleDir(transform.forward, heading, transform.up);

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
