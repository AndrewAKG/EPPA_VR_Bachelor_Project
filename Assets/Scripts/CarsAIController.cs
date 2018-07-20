using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAIController : MonoBehaviour {

    private Renderer carRenderer;
    private List<Transform> nodes;
    private int currentNode = 0;
    private bool carStateChanged = false;

    public Transform path;
    [Header("Car Engine")]
    public float maxSteeringAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float maxMotorTorque = 50f;
    public float maxBrakingTorque = 200f;
    public float currentSpeed;
    public float maxSpeed = 80f;
    public bool isBraking = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    [SerializeField]
    public Vector3 frontSensorOffset;

	// Use this for initialization
	void Start () {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathTransforms.Length; i++)
        {
            if(pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        print(GetComponent<Renderer>().bounds.size);
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
            if (hit.collider.gameObject.CompareTag("TLS") || hit.collider.gameObject.CompareTag("TLS2"))
            {
                TrafficLight light = hit.collider.gameObject.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                if (trafficState == 1 || trafficState == 2)
                {
                    isBraking = true;
                    carStateChanged = true;
                }
            }

            if (hit.collider.gameObject.CompareTag("AICar"))
            {
                isBraking = true;
                carStateChanged = true;
            }
        }

        // front right sensor
        sensorStartPos += transform.right * frontSensorOffset.x;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("TLS") || hit.collider.gameObject.CompareTag("TLS2"))
            {
                TrafficLight light = hit.collider.gameObject.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                if (trafficState == 1 || trafficState == 2)
                {
                    isBraking = true;
                    carStateChanged = true;
                }
            }

            if (hit.collider.gameObject.CompareTag("AICar"))
            {
                isBraking = true;
                carStateChanged = true;
            }
        }

        // front left sensor
        sensorStartPos -= transform.right * frontSensorOffset.x * 2;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("TLS") || hit.collider.gameObject.CompareTag("TLS2"))
            {
                TrafficLight light = hit.collider.gameObject.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                if (trafficState == 1 || trafficState == 2)
                {
                    isBraking = true;
                    carStateChanged = true;
                }
            }

            if (hit.collider.gameObject.CompareTag("AICar"))
            {
                isBraking = true;
                carStateChanged = true;
            }
        }

        if (!carStateChanged && isBraking)
        {
            isBraking = false;
        }

        carStateChanged = false;
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
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

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
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
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
            wheelFL.brakeTorque = maxBrakingTorque;
            wheelFR.brakeTorque = maxBrakingTorque;
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
