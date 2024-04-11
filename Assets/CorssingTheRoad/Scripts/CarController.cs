using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Variables for car movement
    public float speed;
    public float minSpeed = 1f;
    public float maxSpeed = 2f;
    public float stopDistance = 2f; //Distance at which the car stops behind another car
    public float playerDistance = 1f; //Distance at which the car stops before the player
    public float accelerationRate = 0.1f; //Rate at which the car accelerates

    private bool isMoving = true; // Flag to indicate whether the car is moving

    public float carFront = 5f;
    public GameObject carFrontEmpty;
    // Method to set the speed of the car
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        //Move the car forward along its local z-axis
        if (isMoving)
        {
            // Move the car forward along its local z-axis
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        // Check for obstacles (cars) in front of the car
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, stopDistance))
        {
            if (hit.collider.CompareTag("Car"))
            {
                // If the distance to the car in front is less than stopDistance, stop the car
                isMoving = false;
            }
        }
        else
        {
            // If there are no obstacles ahead, accelerate the car
            isMoving = true;
            Accelerate();
        }

        // Check for obstacles (cars) in front of the car
        RaycastHit hit2;
        if (Physics.SphereCast(carFrontEmpty.transform.position, carFront, carFrontEmpty.transform.forward, out hit2, playerDistance))
        {
            if (hit2.collider.CompareTag("Player"))
            {
                speed = 0f;
                //Add a LoseScreen from a ScreensActivation
                ScreensActivation.instance.ActivateLoseScreen();
                
            }
        }
    }

    void Accelerate()
    {
        // Accelerate the car gradually
        if (speed < maxSpeed)
        {
            speed += accelerationRate * Time.deltaTime;
        }
        else
        {
            speed = maxSpeed; // Ensure speed doesn't exceed maxSpeed
        }
    }

    // Method to handle destroying the car when it reaches a certain point
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyPoint"))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Car triggered something with tag: " + other.tag);
        }
    }
    void OnDrawGizmos()
    {
        // Draw the raycast in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * stopDistance);

        // Draw the sphere cast in the scene view for debugging
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * playerDistance, carFront);
    }
}

