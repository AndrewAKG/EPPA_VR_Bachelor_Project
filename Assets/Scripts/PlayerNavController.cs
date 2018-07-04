using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavController : MonoBehaviour {

    private Animator anim;
    public bool isWalking = false;
    public float speed;
    public Vector3 originalPos;
    public bool once = false;

   

    void Start()
    {
        anim = GetComponent<Animator>();
        originalPos = transform.position;
    }

    void Update()
    {
        if (!once)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerGoToSM"))
            {
                Debug.Log("Entered");
            }
            else
            {
                transform.position = originalPos;
                once = true;
            }
        }
        
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        transform.Rotate(0, x, 0);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!isWalking)
            {
                isWalking = true;
                anim.Play("M_walk");
            }
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            transform.Translate(0, 0, z); // Only move when "up arrow" is pressed.
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
