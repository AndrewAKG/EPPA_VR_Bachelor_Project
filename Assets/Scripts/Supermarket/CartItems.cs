using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartItems : MonoBehaviour
{
    public Transform activeCart;
    private Renderer cartRenderer;
    private bool started = false;
    private bool[] RequiredItems;
    //private bool appleJuice = false;
    //private bool cookies = false;

    private void Start()
    {
        started = true;
        cartRenderer = activeCart.GetComponent<Renderer>();
        print(cartRenderer.bounds.size);
    }

    // Update is called once per frame
    void Update () {
		
	}

    //public void checkItems(Vector3 targetPos)
    //{
    //    var hitColliders = Physics.OverlapSphere(targetPos, 6.1f);
    //    if (hitColliders.Length > 0)
    //    {
    //        foreach (Collider hitCollider in hitColliders)
    //        {
    //            if (hitCollider.gameObject.CompareTag("AppleJuice"))
    //            {
    //                appleJuice = true;
    //            }

    //            if (hitCollider.gameObject.CompareTag("Cookies"))
    //            {
    //                cookies = true;
    //            }
    //        }
    //    }

    //    if
    //}

    private void OnDrawGizmos()
    {
        if (started)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeCart.position, 6.1f);
        }
    }
}
