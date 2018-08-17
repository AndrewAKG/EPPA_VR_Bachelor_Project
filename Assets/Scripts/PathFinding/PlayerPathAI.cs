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
    private Vector3 sensorOrigin;
    private Vector3 sensorOffset = new Vector3(0f, 0.9f, 0.1f);
    private float frontSensorAngle = 30f;
    CapsuleCollider playerCollider;
    Collider other;

    [SerializeField]
    private AudioSource hi;

    [SerializeField]
    private AudioSource left;

    [SerializeField]
    private AudioSource right;

    [SerializeField]
    private AudioSource straight;

    [SerializeField]
    private AudioSource pedestrianRed;

    // Use this for initialization
    void Start () {
        pathFinder = GetComponent<PathFinding>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCollider = player.GetComponent<CapsuleCollider>();
        target = GameObject.FindGameObjectWithTag("Supermarket").transform;
        agentCanvas = GameObject.FindGameObjectWithTag("AgentCanvas");
        agent = GameObject.FindGameObjectWithTag("Agent");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoFindPath();
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

        //RaycastHit hit;
        //Vector3 fwd = player.transform.TransformDirection(Vector3.forward);
        //sensorOrigin = player.transform.position;
        //sensorOrigin += player.transform.forward * sensorOffset.z;
        //sensorOrigin += player.transform.up * sensorOffset.y;

        //if (Physics.Raycast(sensorOrigin, fwd, out hit, 1f))
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
        //            StartCoroutine(WarnRedLight());
        //        }
        //    }
        //}
    }

    IEnumerator WarnRedLight()
    {
        showAgentCanvas = true;
        agent.GetComponent<Animator>().SetBool("RedLight", true);
        pedestrianRed.Play();
        yield return new WaitForSeconds(5);
        agent.GetComponent<Animator>().SetBool("RedLight", false);
        yield return new WaitForSeconds(1);
        showAgentCanvas = false;
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
        //Vector3 nearestNodeHeading = path[0] - player.position;
        int destinationDirection = AngleDir(player.forward, destinationHeading, player.up);
        //int nearestNodeDirection = AngleDir(player.forward, nearestNodeHeading, player.up);
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
                yield return null;
                break;
        }

        yield return new WaitForSeconds(1);
        showAgentCanvas = false;
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
