using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLSController : MonoBehaviour {

    Material m_Material;
    Material m_red;
    Material m_yellow;
    Material m_green;
    public int state = 0;
    Renderer rend;
    Renderer rend_m;
    GameObject m;

    void Start()
    {
        rend = GetComponent<Renderer>();
        m_red = Resources.Load("TL_red") as Material;
        m_yellow = Resources.Load("TL_yellow") as Material;
        m_green = Resources.Load("TL_green") as Material;
        //m = GameObject.FindGameObjectWithTag("Moshah");
        //rend_m = m.GetComponent<Renderer>();
        StartCoroutine(ChangeLights());
        //Fetch the Material from the Renderer of the GameObject
        //m_Material = GetComponent<Renderer>().material;
    }

    IEnumerator ChangeLights()
    {
        while (true)
        {
            state = 1;
            rend.material = m_red;
            yield return new WaitForSeconds(15);
            state = 2;
            rend.material = m_yellow;
            yield return new WaitForSeconds(3);
            state = 3;
            rend.material = m_green;
            yield return new WaitForSeconds(20);
        }
    }

    // Update is called once per frame
    void Update () { 
        //print(rend.material.name);
        //rend.material = m_red;
        //rend_m.material = m_green;
    }
}
