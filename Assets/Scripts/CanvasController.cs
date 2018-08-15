﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    private GameObject agentCanvas;
    private GameObject agent;
    private bool showAgentCanvas = false;

	// Use this for initialization
	void Start () {
        agentCanvas = GameObject.FindGameObjectWithTag("AgentCanvas");
        agent = GameObject.FindGameObjectWithTag("Agent");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            showAgent();
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
	}

    public void showAgent()
    {
        showAgentCanvas = !showAgentCanvas;
    }
}
