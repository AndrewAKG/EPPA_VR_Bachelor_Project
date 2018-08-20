using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CartItems : MonoBehaviour
{
    public Transform activeCart;
    private Renderer cartRenderer;
    private bool started = false;
    private Dictionary<string, bool> requiredItems;
    private Vector3 checkingSensorOffset = new Vector3(0f, 0.6f, -0.1f);
    private bool showAgentCanvas = false;
    private GameObject agentCanvas;
    private GameObject agent;
    private GameObject mobileCanvas;
    private GameObject mobileImage;
    private bool showMobileCanvas = false;
    private int itemsSoFar = 0;
    private bool distractOnce = false;

    [SerializeField]
    private AudioSource buy;

    [SerializeField]
    private AudioSource OrangeJuice;

    [SerializeField]
    private AudioSource LemonJuice;

    [SerializeField]
    private AudioSource Milk;

    [SerializeField]
    private AudioSource Meat;

    [SerializeField]
    private AudioSource Fish;

    [SerializeField]
    private AudioSource PizzaMozzarella;

    [SerializeField]
    private AudioSource Fruit;

    [SerializeField]
    private AudioSource Tee;

    [SerializeField]
    private AudioSource MobileRinging;

    private void Start()
    {
        requiredItems = new Dictionary<string, bool>();
        started = true;
        cartRenderer = activeCart.GetComponent<Renderer>();
        agent = GameObject.FindGameObjectWithTag("Agent");
        mobileImage = GameObject.FindGameObjectWithTag("MobileImage");
        agentCanvas = GameObject.FindGameObjectWithTag("AgentCanvas");
        mobileCanvas = GameObject.FindGameObjectWithTag("MobileCanvas");
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
        requiredItems.Add("Pizza Mozzarella", false);
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
    private void Update () {
        checkItems(transform.position + checkingSensorOffset);

        if (showAgentCanvas)
        {
            agent.SetActive(true);
            //agentCanvas.GetComponent<CanvasGroup>().alpha = 1f;
            //agent.transform.localScale = Vector3.one;
        }
        else
        {
            agent.SetActive(false);
            //agentCanvas.GetComponent<CanvasGroup>().alpha = 0f;
            //agent.transform.localScale = Vector3.zero;
        }

        if (showMobileCanvas)
        {
            mobileImage.SetActive(true);
            //mobileCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            mobileImage.SetActive(false);
            //mobileCanvas.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    private void MobileDistraction()
    {
        MobileRinging.Play();
        showMobileCanvas = true;
    }

    public void EndDistraction()
    {
        MobileRinging.Stop();
        showMobileCanvas = false;
    }

    private void checkItems(Vector3 targetPos)
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
        }

        foreach (KeyValuePair<string, bool> kvp in requiredItems)
        {
            if (kvp.Value)
            {
                itemsSoFar++;
            }
        }

        if (!distractOnce)
        {
            if(itemsSoFar >= 4)
            {
                MobileDistraction();
                distractOnce = true;
            }
        }

        if(itemsSoFar == 8)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }

        itemsSoFar = 0;
    }

    public void CallAgent()
    {
        StartCoroutine(RecognizeMissingItems());
    }

    IEnumerator RecognizeMissingItems()
    {
        showAgentCanvas = true;
        buy.Play();
        yield return new WaitForSeconds(1.5f);

        if (!requiredItems["Orange Juice"])
        {
            OrangeJuice.Play();
            yield return new WaitForSeconds(1.6f);
        }

        if (!requiredItems["Lemon Juice"])
        {
            LemonJuice.Play();
            yield return new WaitForSeconds(1.2f);
        }

        if (!requiredItems["Milk"])
        {
            Milk.Play();
            yield return new WaitForSeconds(1);
        }

        if (!requiredItems["Fish"])
        {
            Fish.Play();
            yield return new WaitForSeconds(1);
        }

        if (!requiredItems["Pizza Mozzarella"])
        {
            PizzaMozzarella.Play();
            yield return new WaitForSeconds(1.6f);
        }

        if (!requiredItems["Meat"])
        {
            Meat.Play();
            yield return new WaitForSeconds(1);
        }

        if (!requiredItems["Tee"])
        {
            Tee.Play();
            yield return new WaitForSeconds(1);
        }

        if (!requiredItems["Fruit"])
        {
            Fruit.Play();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
        showAgentCanvas = false;
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
