using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CartItems : MonoBehaviour
{
    public Transform activeCart;
    private Renderer cartRenderer;
    private bool started = false;
    private Dictionary<string, bool> requiredItems;
    private Vector3 checkingSensorOffset = new Vector3(0f, 0.6f, -0.1f);

    private void Start()
    {
        requiredItems = new Dictionary<string, bool>();
        started = true;
        cartRenderer = activeCart.GetComponent<Renderer>();
        //print(cartRenderer.bounds.size);
        addInitialValues();
    }

    private void addInitialValues()
    {
        requiredItems.Add("Orange Juice", false);
        requiredItems.Add("Lemon Juice", false);
        requiredItems.Add("Milk", false);
        requiredItems.Add("Fish", false);
        requiredItems.Add("Meat", false);
        requiredItems.Add("Pizza Mozarella", false);
        requiredItems.Add("Cheese", false);
        requiredItems.Add("Fruit", false);
        requiredItems.Add("Tee", false);
    }

    private bool CheckCartItem(Collider[] hitColliders, string tag)
    {
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
        checkItems(transform.position + checkingSensorOffset);
	}

    public void checkItems(Vector3 targetPos)
    {
        var hitColliders = Physics.OverlapSphere(targetPos, 0.6f);
        if (hitColliders.Length > 0)
        {
           requiredItems["Orange Juice"] = CheckCartItem(hitColliders, "OrangeJuice");
           requiredItems["Lemon Juice"] = CheckCartItem(hitColliders, "LemonJuice");
           requiredItems["Milk"] = CheckCartItem(hitColliders, "Milk");
           requiredItems["Fish"] = CheckCartItem(hitColliders, "Fish");
           requiredItems["Meat"] = CheckCartItem(hitColliders, "Meat");
           requiredItems["Pizza Mozzarella"] = CheckCartItem(hitColliders, "PizzaMozzarella");
           requiredItems["Fruit"] = CheckCartItem(hitColliders, "Fruit");
           requiredItems["Tee"] = CheckCartItem(hitColliders, "Tee");
           requiredItems["Cheese"] = CheckCartItem(hitColliders, "Cheese");
        }

        foreach(KeyValuePair<string, bool> kvp in requiredItems)
        {
            print("Key: " + kvp.Key + " Value: " + kvp.Value);
        }
    }

    private void OnDrawGizmos()
    {
        if (started)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(activeCart.position + checkingSensorOffset, 0.6f);
        }
    }
}
