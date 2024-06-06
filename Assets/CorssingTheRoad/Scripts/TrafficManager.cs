using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPointParameters
{
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 5f;
    public float initialSpawnDelay = 1f;        //If we want to have an increase
    public float spawnDelayIncreaseRate = 0.1f; //If we want to make it easier for the player we can increase the spawn delay overtime
                                                //This way the traffic gets less with time passing and it is not fully random
    public float spawnDistance = 2f;            // Distance at which the traffic manager checks for space to spawn a new car

    public Vector3 spawnBoxSize = new Vector3(1f, 1f, 2f); // Size of the box to check for space
}

public class TrafficManager : MonoBehaviour
{
    public GameObject[] carPrefabs;  //Cars List
    public AudioClip[] carSounds;    //Car Sounds List
    public Transform[] spawnPoints;  //Option to add more lanes
    public SpawnPointParameters[] spawnPointParameters; // Parameters for each spawn point
    //public LayerMask carLayerMask;   // Layer mask for cars to ensure raycast only checks cars
    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            if (ScreensActivation.instance != null && ScreensActivation.instance.isActive)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    float spawnDelay = GetSpawnDelay(i); //Pass index to get parameters for the specific spawn point
                    yield return new WaitForSeconds(spawnDelay);

                    // Check if there's enough space to spawn a car
                    if (CheckSpaceForCar(i)) //Pass index to check space for the specific spawn point
                    {
                        // Spawn a car
                        SpawnCar(i);  //Pass index to spawn car at the specific spawn point
                    }
                }
            }
            else
            {
                break;
            }
        }
    }

    private float GetSpawnDelay(int index)
    {
        // Example: Increase spawn delay gradually over time
        float time = Time.timeSinceLevelLoad;

        // Define the duration of time for increased spawn rate (in seconds)
        float initialSpawnDuration = 4f; // Adjust as needed

        // Check if the elapsed time is within the initial spawn duration the idea is to have the cars at first spawn a bit more often and then be random between min and max value
        if (time < initialSpawnDuration)
        {
            // Gradually decrease the spawn delay during the initial spawn duration
            float decreasedDelay = Mathf.Clamp(spawnPointParameters[index].initialSpawnDelay + time * spawnPointParameters[index].spawnDelayIncreaseRate, spawnPointParameters[index].minSpawnDelay, spawnPointParameters[index].maxSpawnDelay);
            return decreasedDelay;
        }
        else
        {
            // Return a random spawn delay within the specified range
            return Random.Range(spawnPointParameters[index].minSpawnDelay, spawnPointParameters[index].maxSpawnDelay);
        }
    }
    
    //In the end i had to add a tag compare in the overlap box (btw all models need colldiers)
    private bool CheckSpaceForCar(int index)
    {
        foreach (var car in Physics.OverlapBox(spawnPoints[index].position, spawnPointParameters[index].spawnBoxSize / 2)) //The spawn box size in the inspector has to be 4 or 5 as
                                                                                                                           //some models are quite big and need more space for their
                                                                                                                           //sphere collider to detect the car in front
        {
            if (car.CompareTag("Car"))
            {
                return false; // Return false if any car collider is detected within the spawn area
            }
        }
        return true; // Return true if no car collider is detected within the spawn area
    }
   
    private void SpawnCar(int index)
    {
        // Randomly select a car prefab
        GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // Instantiate the selected car prefab
        GameObject newCar = Instantiate(randomCarPrefab, spawnPoints[index].position, spawnPoints[index].rotation);

        // Get the CarController component from the spawned car
        CarController carController = newCar.GetComponent<CarController>();

        ScreensActivation.instance.carsInScene.Add(carController);

        // Set the speed of the car using the CarController's SetSpeed method
        carController.SetSpeed(Random.Range(carController.minSpeed, carController.maxSpeed));

        // Randomly select a car sound
        AudioClip randomCarSound = carSounds[Random.Range(0, carSounds.Length)];

        // Attach the selected car sound to the car
        AudioSource audioSource = newCar.GetComponent<AudioSource>();
        if (audioSource != null && randomCarSound != null)
        {
            audioSource.clip = randomCarSound;
            audioSource.loop = true; //Loop audio insurance
            audioSource.Play();
        }
    }
    private void OnDrawGizmos()
    {
        if (spawnPoints == null || spawnPointParameters == null) return;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] != null)
            {
                Gizmos.color = CheckSpaceForCar(i) ? Color.green : Color.red;
                Vector3 boxCenter = spawnPoints[i].position + spawnPoints[i].forward * (spawnPointParameters[i].spawnBoxSize.z / 2);
                Gizmos.DrawWireCube(boxCenter, spawnPointParameters[i].spawnBoxSize);
            }
        }
    }
}