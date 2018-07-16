using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAIController : MonoBehaviour {

    private Renderer carRenderer;
    private List<Transform> nodes;
    private int currentNode = 0;

    public Transform path;
    [Header("Car Engine")]
    public float maxSteeringAngle = 40f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float maxMotorTorque = 80f;
    public float maxBrakingTorque = 200f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public bool isBraking = false;

    [Header("Sensors")]
    public float sensorLength = 3.5f;
    public Vector3 frontSensorOffset = new Vector3(1.15f, 0.75f, 2.4f);
    //public float frontSideOffset = 1.15f;

	// Use this for initialization
	void Start () {
        //carRenderer = GetComponent<Renderer>();
        //print(carRenderer.bounds.size);

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if(pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
        print(nodes.Count);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        ApplySensors();
        ApplySteer();
        Drive();
        CheckNextPointDistance();
        Braking();
	}

    private void ApplySensors()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorOffset.z;
        sensorStartPos += transform.up * frontSensorOffset.y;

        // front center sensor
        if(Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("TLS"))
            {
                TLSController controller = hit.collider.gameObject.GetComponent<TLSController>();
                int trafficState = controller.state;

                if (trafficState == 1 || trafficState == 2)
                {
                    hit.collider.enabled = true;
                    print("State: " + trafficState);
                    isBraking = true;
                }
                else if (trafficState == 3)
                {
                    hit.collider.enabled = false;
                    print("State: " + trafficState);
                    isBraking = false;
                }
            }
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // front right sensor
        sensorStartPos += transform.right * frontSensorOffset.x;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // front left sensor
        sensorStartPos -= transform.right * frontSensorOffset.x * 2;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
        }

    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteeringAngle;
        wheelFR.steerAngle = newSteer;
        wheelFL.steerAngle = newSteer;
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 100;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
            wheelRL.motorTorque = maxMotorTorque;
            wheelRR.motorTorque = maxMotorTorque;
        } else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
    }

    private void CheckNextPointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 0.0f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            //wheelFL.brakeTorque = maxBrakingTorque;
            //wheelFR.brakeTorque = maxBrakingTorque;
            wheelRL.brakeTorque = maxBrakingTorque;
            wheelRR.brakeTorque = maxBrakingTorque;
        }
        else
        {
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }
}
