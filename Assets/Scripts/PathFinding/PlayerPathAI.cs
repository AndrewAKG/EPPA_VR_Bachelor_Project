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

    [SerializeField]
    private AudioSource hi;

    [SerializeField]
    private AudioSource left;

    [SerializeField]
    private AudioSource right;

    [SerializeField]
    private AudioSource straight;

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

    public void GoFindPath()
    {
        path = pathFinder.FindPath(player.position, target.position);
        if (path.Length > 0)
        {
            StartCoroutine(OnPathFound(path));
        }
        else
        {
            print("No Path Found");
        }
    }

    IEnumerator OnPathFound(Vector3[] newPath)
    {
        path = newPath;
        Vector3 destinationHeading = target.position - player.position;
        Vector3 nearestNodeHeading = path[0] - player.position;
        int destinationDirection = AngleDir(player.forward, destinationHeading, player.up);
        int nearestNodeDirection = AngleDir(player.forward, nearestNodeHeading, player.up);
        showAgentCanvas = true;

        switch (destinationDirection)
        {
            case -1:
                agent.GetComponent<Animator>().SetBool("FoundPath", true);
                hi.Play();
                yield return new WaitForSeconds(1);
                left.Play();
                yield return new WaitForSeconds(2);
                agent.GetComponent<Animator>().SetBool("FoundPath", false);
                break;
            case 1:
                agent.GetComponent<Animator>().SetBool("FoundPath", true);
                hi.Play();
                yield return new WaitForSeconds(1);
                right.Play();
                yield return new WaitForSeconds(1.5f);
                agent.GetComponent<Animator>().SetBool("FoundPath", false);
                break;
            case 0:
                agent.GetComponent<Animator>().SetBool("FoundPath", true);
                hi.Play();
                yield return new WaitForSeconds(1);
                straight.Play();
                yield return new WaitForSeconds(2);
                agent.GetComponent<Animator>().SetBool("FoundPath", false);
                break;
            default:
                print("Nothing");
                break;
        }

        showAgentCanvas = false;
        yield return null;
    }

    int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        print(dir);

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
