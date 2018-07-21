using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarsSpawner : MonoBehaviour {

    private Vector3 SpawningPosition;
    //private CarsAIController carController;
    private Transform carPath;
    private Vector3 carStartPosition;
    private Quaternion carStartRotation;
    private GameObject spawnedCar;
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
        while(carsCount < 3)
        {
            yield return new WaitForSeconds(10);
            InstantiateCar();
        }
    }

    private void InstantiateCar()
    {
        //int randomIndex = (int)(Random.Range(0, 2));
        //print(randomIndex);
        int randomCar = (int)(Random.Range(0, cars.Length - 1));
        //print(randomCar);
        carPath = carsPaths[randomIndex];
        //print(carPath);
        carStartPosition = spawningPoints[randomIndex].position;
        //print(carStartPosition);
        carStartRotation = spawningPoints[randomIndex].rotation;
        spawnedCar = new GameObject();
        spawnedCar = cars[randomCar];
        //print(spawnedCar);
        spawnedCar.GetComponent<CarsAIController>().path = carPath;
        //print(spawnedCar.GetComponent<CarsAIController>().path);
        Instantiate(spawnedCar, carStartPosition, carStartRotation);
        carsCount++;

        if(randomIndex == spawningPoints.Length - 1)
        {
            randomIndex = 0;
        }
        else
        {
            randomIndex++;
        }
    }
}
