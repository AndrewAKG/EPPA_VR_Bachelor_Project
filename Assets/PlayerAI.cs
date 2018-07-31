using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{

    private Vector3 sensorOrigin;
    private Vector3 sensorOffset = new Vector3(0f, 0.9f, 0.1f);
    private float frontSensorAngle = 30f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        sensorOrigin = transform.position;
        sensorOrigin += transform.forward * sensorOffset.z;
        sensorOrigin += transform.up * sensorOffset.y;

        if (Physics.Raycast(sensorOrigin, fwd, out hit, 2f))
        {
            Debug.DrawLine(sensorOrigin, hit.point);
        }
    }
}
