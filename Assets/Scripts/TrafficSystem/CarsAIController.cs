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
    public float maxSteeringAngle;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float maxMotorTorque;
    public float maxBrakingTorque;
    public float currentSpeed;
    public float maxSpeed;
    public bool isBraking = false;

    [Header("Sensors")]
    public float sensorLength = 5f;
    [SerializeField]
    public Vector3 frontSensorOffset;

    public CarsAIController(Transform path)
    {
        this.path = path;
    }

	// Use this for initialization
	void Start () {
        if (path != null)
        {
            Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
            nodes = new List<Transform>();

            for (int i = 0; i < pathTransforms.Length; i++)
            {
                if (pathTransforms[i] != path.transform)
                {
                    nodes.Add(pathTransforms[i]);
                }
            }
        }
        
        //print(GetComponent<Renderer>().bounds.size);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ApplySensors();
        if(path != null)
        {
            ApplySteer();
            Drive();
            CheckNextPointDistance();
        }
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
            if (hit.collider.gameObject.CompareTag("TLSHC") || hit.collider.gameObject.CompareTag("TLSVC"))
            {
                GameObject collisionObject = hit.collider.gameObject;
                GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
                TrafficLight light = collisionObjectParent.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                float angle = Vector3.Angle(collisionObject.transform.forward, transform.position - collisionObject.transform.position);
                //print(angle);
                bool frontFacing = (angle <= 10);

                if ((trafficState == 1 || trafficState == 2) && frontFacing)
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

            //if (hit.collider.gameObject.CompareTag("AIPedestrian"))
            //{
            //    isBraking = true;
            //    carStateChanged = true;
            //}

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                isBraking = true;
                carStateChanged = true;
            }
        }

        // front right sensor
        sensorStartPos += transform.right * frontSensorOffset.x;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("TLSHC") || hit.collider.gameObject.CompareTag("TLSVC"))
            {
                GameObject collisionObject = hit.collider.gameObject;
                GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
                TrafficLight light = collisionObjectParent.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                float angle = Vector3.Angle(collisionObject.transform.forward, transform.position - collisionObject.transform.position);
                //print(angle);
                bool frontFacing = (angle <= 10);

                if ((trafficState == 1 || trafficState == 2) && frontFacing)
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

            //if (hit.collider.gameObject.CompareTag("AIPedestrian"))
            //{
            //    isBraking = true;
            //    carStateChanged = true;
            //}

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                isBraking = true;
                carStateChanged = true;
            }
        }

        // front left sensor
        sensorStartPos -= transform.right * frontSensorOffset.x * 2;
        if (Physics.Raycast(sensorStartPos, fwd, out hit, sensorLength))
        {
            if (hit.collider.gameObject.CompareTag("TLSHC") || hit.collider.gameObject.CompareTag("TLSVC"))
            {
                GameObject collisionObject = hit.collider.gameObject;
                GameObject collisionObjectParent = collisionObject.transform.parent.gameObject;
                TrafficLight light = collisionObjectParent.GetComponent<TrafficLight>();
                int trafficState = light.getState();

                float angle = Vector3.Angle(collisionObject.transform.forward, transform.position - collisionObject.transform.position);
                //print(angle);
                bool frontFacing = (angle <= 10);

                if ((trafficState == 1 || trafficState == 2) && frontFacing)
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

            //if (hit.collider.gameObject.CompareTag("AIPedestrian"))
            //{
            //    isBraking = true;
            //    carStateChanged = true;
            //}

            if (hit.collider.gameObject.CompareTag("Player"))
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
        } else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
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
