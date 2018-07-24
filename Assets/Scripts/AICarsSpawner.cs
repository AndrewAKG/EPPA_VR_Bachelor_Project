using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarsSpawner : MonoBehaviour {

    private Vector3 SpawningPosition;
    private Transform carPath;
    private Vector3 carStartPosition;
    private Quaternion carStartRotation;
    private GameObject carType;
    private int carsCount = 0;
    private int randomIndex = 0;
    private int randomCarType = 0;
    private bool started = false;

    [SerializeField]
    public Transform[] spawningPoints;
    [SerializeField]
    public Transform[] carsPaths;
    [SerializeField]
    public GameObject[] cars;

	// Use this for initialization
	void Start () {
        started = true;
        StartCoroutine(SpawnCars());
	}

    IEnumerator SpawnCars()
    {
        while(carsCount < 50)
        {
            InstantiateCar();
            yield return new WaitForSeconds(3);
        }
    }

    private void InstantiateCar()
    {
        carPath = carsPaths[randomIndex];
        carStartPosition = spawningPoints[randomIndex].position;
        carStartRotation = spawningPoints[randomIndex].rotation;
        carType = cars[randomCarType];
        if (checkIfPosEmpty(carStartPosition))
        {
            GameObject spawnedCar = Instantiate(carType, carStartPosition, carStartRotation);
            spawnedCar.gameObject.tag = "AICar";
            CarsAIController carController = spawnedCar.GetComponent<CarsAIController>();
            carController.path = carPath;
            carsCount++;
            randomCarType++;
        }
        randomIndex++;

        if (randomIndex == spawningPoints.Length)
        {
            randomIndex = 0;
        }

        if (randomCarType == cars.Length)
        {
            randomCarType = 0;
        }
    }

    public bool checkIfPosEmpty(Vector3 targetPos)
    {
        var hitColliders = Physics.OverlapSphere(targetPos, 5);
        if (hitColliders.Length > 0)
        {
            foreach(Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("AICar"))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        if (started)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawningPoints[randomIndex].position, 6.1f);
        }
    }
}
