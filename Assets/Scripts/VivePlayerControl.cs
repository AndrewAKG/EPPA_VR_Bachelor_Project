using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VivePlayerControl : MonoBehaviour
{

    public GameObject player;
    public Animator anim;
    public bool isWalking = false;
    public float speed;
    public float sensitivityX;

    // 1
    private SteamVR_TrackedObject trackedObj;

    Vector2 touchpad;
    // 2
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {

        //If finger is on touchpad
        if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        { 
            if (!isWalking)
            {
                isWalking = true;
            }
            //Read the touchpad values
            touchpad = Controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            var z = touchpad.y * Time.deltaTime * speed;
            if(z > 0)
            {
                anim.Play("M_walk");
                player.transform.Translate(0, 0, z); // Only move when upon pressed.
            }

            if (touchpad.x > 0.3f || touchpad.x < -0.3f)
            {
                player.transform.Rotate(0, touchpad.x * sensitivityX, 0);
            }
        }
        else
        {
            if (isWalking)
            {
                anim.Play("M_idle1");
            }
            isWalking = false;
        }

        //if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
        //{
        //    touchpad = Controller.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        //    var y = touchpad.x * Time.deltaTime * sensitivityX;
        //    player.transform.Rotate(0, y, 0);
        //}
    }
}