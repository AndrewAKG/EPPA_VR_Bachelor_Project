using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLSManager : MonoBehaviour {

    private GameObject[] TLSLights;
    private GameObject[] TLS2Lights;
    private GameObject[] TLSPLights;
    private int redSeconds = 20;
    private int yellowSeconds = 3;
    private int greenSeconds = 20;
    private int pedestrianSeconds = 15;

    private void Start()
    {
        TLSLights = GameObject.FindGameObjectsWithTag("TLS");
        TLS2Lights = GameObject.FindGameObjectsWithTag("TLS2");
        TLSPLights = GameObject.FindGameObjectsWithTag("TLSP");
        StartCoroutine(ControlTraffic());
    }

    IEnumerator ControlTraffic()
    {
        while (true)
        {
            UpdateTrafficLights(TLSLights, 1);
            UpdateTrafficLights(TLS2Lights, 3);
            UpdatePedestrianLights(TLSPLights, 1);
            yield return new WaitForSeconds(redSeconds);

            UpdateTrafficLights(TLSLights, 2);
            UpdateTrafficLights(TLS2Lights, 2);
            UpdatePedestrianLights(TLSPLights, 1);
            yield return new WaitForSeconds(yellowSeconds);

            UpdateTrafficLights(TLSLights, 3);
            UpdateTrafficLights(TLS2Lights, 1);
            UpdatePedestrianLights(TLSPLights, 1);
            yield return new WaitForSeconds(greenSeconds);

            UpdateTrafficLights(TLSLights, 1);
            UpdateTrafficLights(TLS2Lights, 1);
            UpdatePedestrianLights(TLSPLights, 2);
            yield return new WaitForSeconds(pedestrianSeconds);
        }
    }

    private void UpdateTrafficLights(GameObject[] array, int state)
    {
        foreach (GameObject light in array)
        {
            TrafficLight controller = light.GetComponent<TrafficLight>();
            controller.setState(state);
        }
    }

    private void UpdatePedestrianLights(GameObject[] array, int state)
    {
        foreach (GameObject light in array)
        {
            PedestrianLight controller = light.GetComponent<PedestrianLight>();
            controller.setState(state);
        }
    }

}
