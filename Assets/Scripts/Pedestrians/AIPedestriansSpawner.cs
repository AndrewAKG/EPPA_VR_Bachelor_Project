using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPedestriansSpawner : MonoBehaviour {

    private int pedestriansCount = 0;
    private int randomIndex = 0;
    private int randomPathIndex = 0;
    private int randomPedestrian = 0;
    private Vector3 SpawningPosition;
    private Transform pedestrianPath;
    private Vector3 pedestrianStartPosition;
    private Quaternion pedestrianStartRotation;
    private GameObject pedestrianType;

    [SerializeField]
    public Transform[] spawningPoints;

    [SerializeField]
    public Transform[] pedestriansPaths;

    [SerializeField]
    public GameObject[] pedestrians;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnPedestrians());
    }

    IEnumerator SpawnPedestrians()
    {
        while (pedestriansCount < 100)
        {
            InstantiatePedestrian();
            yield return new WaitForSeconds(1);
        }
    }

    private void InstantiatePedestrian()
    {
        pedestrianPath = pedestriansPaths[randomPathIndex];
        pedestrianStartPosition = spawningPoints[randomIndex].position;
        pedestrianStartRotation = spawningPoints[randomIndex].rotation;
        pedestrianType = pedestrians[randomPedestrian];

        GameObject spawnedpedestrian = Instantiate(pedestrianType, pedestrianStartPosition, pedestrianStartRotation);
        spawnedpedestrian.gameObject.tag = "AIPedestrian";
        PedestrianAI pedestrianController = spawnedpedestrian.GetComponent<PedestrianAI>();
        pedestrianController.path = pedestrianPath;
        pedestriansCount++;
        randomPedestrian++;
        randomIndex++;
        randomPathIndex++;

        if (randomIndex == spawningPoints.Length)
        {
            randomIndex = 0;
        }

        if (randomPathIndex == pedestriansPaths.Length)
        {
            randomPathIndex = 0;
        }

        if (randomPedestrian == pedestrians.Length)
        {
            randomPedestrian = 0;
        }
    }
}
