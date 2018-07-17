using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour {

    Material m_Material;
    Material m_red;
    Material m_yellow;
    Material m_green;
    Renderer rend;
    Renderer rend_m;
    GameObject m;

    void Start()
    {
        rend = GetComponent<Renderer>();
        m_red = Resources.Load("TL_red") as Material;
        m_yellow = Resources.Load("TL_yellow") as Material;
        m_green = Resources.Load("TL_green") as Material;
        m = GameObject.FindGameObjectWithTag("Moshah");
        rend_m = m.GetComponent<Renderer>();
        //Fetch the Material from the Renderer of the GameObject
        //m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(rend.material.name);
            rend.material = m_red;
            rend_m.material = m_green;
        }
    }
}
