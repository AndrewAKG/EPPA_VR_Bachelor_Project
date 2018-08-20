using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianLight : MonoBehaviour {

    private Renderer rend;
    Material m_red;
    Material m_green;
    private int state = 0;

    public int getState()
    {
        return state;
    }

    public void setState(int state)
    {
        this.state = state;
    }

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        m_red = Resources.Load("TLS_Red") as Material;
        m_green = Resources.Load("TLS_Green") as Material;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1: rend.material = m_red; break;
            case 2:
                print("GREEN");
                rend.material = m_green; break;
            default:
                print("default");
                rend.material = m_red; break;
        }
    }
}
