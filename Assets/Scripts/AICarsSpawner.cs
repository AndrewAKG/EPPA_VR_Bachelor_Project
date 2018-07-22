using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarsSpawner : MonoBehaviour {

    private Vector3 SpawningPosition;
    //private CarsAIController carController;
    private Transform carPath;
    private Vector3 carStartPosition;
    private Quaternion carStartRotation;
    private GameObject carType;
    private int carsCount = 0;
    private int randomIndex = 0;

    [SerializeField]
    public Transform[] spawningPoints;
    [SerializeField]
    public Transform[] carsPaths;
    [SerializeField]
    public GameObject[] cars;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnCars());
	}

    IEnumerator SpawnCars()
    {
        while(carsCount < 12)
        {
            yield return new WaitForSeconds(2);
            InstantiateCar();
        }
    }

    private void InstantiateCar()
    {
        int randomCarType = (int)(Random.Range(0, cars.Length - 1));
        carPath = carsPaths[randomIndex];
        carStartPosition = spawningPoints[randomIndex].position;
        carStartRotation = spawningPoints[randomIndex].rotation;
        carType = cars[randomCarType];
        GameObject spawnedCar = Instantiate(carType, carStartPosition, carStartRotation);
        spawnedCar.gameObject.tag = "AICar";
        CarsAIController carController = spawnedCar.GetComponent<CarsAIController>();
        carController.path = carPath;
        carsCount++;
        randomIndex++;

        if (randomIndex == spawningPoints.Length)
        {
            randomIndex = 0;
        }
    }
}
