using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Shopping : MonoBehaviour {

    public GameObject item;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //if (GetComponent<VRTK_InteractGrab>().GetGrabbedObject != null)
        //{
        //    var controllerEvents = GetComponent<VRTK_ControllerEvents>();
        //    if (controllerEvents.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerPress) {
        //        //Do something on trigger press
        //    }
        //}
        //    GameObject handObjClone = Instantiate(item, controllerGameObj.transform.position, quaternion.identity) as GameObject;
        //handObjClone.transform.parent = null; // spawns the object at the controllers location with no parent.
        //handObjClone.getComponent<VRTK_InteractableObject>().attemptGrab(controllerGameObj);
    }
}
