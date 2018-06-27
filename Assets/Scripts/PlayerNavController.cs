using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavController : MonoBehaviour {

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move);
    }
}
