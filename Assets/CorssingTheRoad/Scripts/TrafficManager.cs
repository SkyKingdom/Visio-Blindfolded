using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    public GameObject[] carPrefabs; //Cars List
    public AudioClip[] carSounds; //Car Sounds List
    public Transform spawnPoint;
    public float minSpawnDelay = 0.5f;
    public float maxSpawnDelay = 5f;
    public float initialSpawnDelay = 1f; //If we want to have an increase
    public float spawnDelayIncreaseRate = 0.1f; //If we want to make it easier for the player we can increase the spawn delay overtime
                                                //This way the traffic gets less with time passing and it is not fully random

    public float spawnDistance = 5f; // Distance at which the traffic manager checks for space to spawn a new car

    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        while (true)
        {
            if (ScreensActivation.instance.isActive)
            {
                float spawnDelay = GetSpawnDelay();
                yield return new WaitForSeconds(spawnDelay);

                // Check if there's enough space to spawn a car
                if (CheckSpaceForCar())
                {
                    // Spawn a car
                    SpawnCar();
                }
            }
            else
            {
                break;
            }
        }
    }

    private float GetSpawnDelay()
    {
        // Example: Increase spawn delay gradually over time
        float time = Time.timeSinceLevelLoad;

        // Define the duration of time for increased spawn rate (in seconds)
        float initialSpawnDuration = 4f; // Adjust as needed

        // Check if the elapsed time is within the initial spawn duration the idea is to have the cars at first spawn a bit more often and then be random between min and max value
    if (time < initialSpawnDuration)
    {
        // Gradually decrease the spawn delay during the initial spawn duration
        float decreasedDelay = Mathf.Clamp(initialSpawnDelay + time * spawnDelayIncreaseRate, minSpawnDelay, maxSpawnDelay);
        return decreasedDelay;
    }
    else
    {
        // Return a random spawn delay within the specified range
        return Random.Range(minSpawnDelay, maxSpawnDelay);
    }
        //return Mathf.Clamp(initialSpawnDelay + time * spawnDelayIncreaseRate, minSpawnDelay, maxSpawnDelay); //If we want to have an increase
        //return Mathf.Clamp(initialSpawnDelay + time, minSpawnDelay, maxSpawnDelay); //No increase but idk
        //return Random.Range(minSpawnDelay, maxSpawnDelay); //I think that's the best way to do it
    }

    private bool CheckSpaceForCar()
    {
        // Cast a ray forward to check for space
        RaycastHit hit;
        if (!Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hit, spawnDistance))
        {
            // There's enough space to spawn a car
            Debug.DrawRay(spawnPoint.position, spawnPoint.forward * spawnDistance, Color.green);
            return true;
        }
        else
        {
            // There's not enough space to spawn a car
            Debug.DrawRay(spawnPoint.position, spawnPoint.forward * hit.distance, Color.black);
            return false;
        }
    }

    private void SpawnCar()
    {
        // Randomly select a car prefab
        GameObject randomCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // Instantiate the selected car prefab
        GameObject newCar = Instantiate(randomCarPrefab, spawnPoint.position, spawnPoint.rotation);

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
}
