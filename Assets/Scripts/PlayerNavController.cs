using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavController : MonoBehaviour {

    private Animator anim;
    public bool isWalking = false;
    public float speed;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        transform.Rotate(0, x, 0);

        if (Input.GetKey("w"))
        {
            if (!isWalking)
            {
                isWalking = true;
                anim.Play("M_walk");
            }
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            transform.Translate(0, 0, z); // Only move when "w" is pressed.
        }
        else
        {
            if (isWalking)
            {
                anim.Play("M_idle1");
            }
            isWalking = false;
        }
    }
}
