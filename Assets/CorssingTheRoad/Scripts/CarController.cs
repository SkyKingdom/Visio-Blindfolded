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

    public bool isMoving = true; // Flag to indicate whether the car is moving

    public bool isThere = true;

    public float carFront = 5f;
    public GameObject carFrontEmpty;

    // Method to set the speed of the car
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
       if (ScreensActivation.instance.isActive)
        {
            //Move the car forward along its local z-axis
            if (isMoving)
            {
                // Move the car forward along its local z-axis
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        if (ScreensActivation.instance.isActive)
        {
            //// Check for obstacles (cars) in front of the car. At first I made it with a ray cast but then i realized cars are different heights and the ray misses
            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, stopDistance))
            //{
            //    if (hit.collider.CompareTag("Car"))
            //    {
            //        // If the distance to the car in front is less than stopDistance, stop the car
            //        isMoving = false;
            //    }
            //}
            // Check for obstacles (cars) in front of the car using sphere cast
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, carFront, transform.forward, out hit, stopDistance))
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

            // Check for obstacles (player) in front of the car
            RaycastHit hit2;
            if (Physics.SphereCast(carFrontEmpty.transform.position, carFront, carFrontEmpty.transform.forward, out hit2, playerDistance))
            {
                if (hit2.collider.CompareTag("Player"))
                {
                    speed = 0f; //Double check i think it could be removed
                    //Add a LoseScreen from a ScreensActivation
                    ScreensActivation.instance.ActivateLoseScreen();

                }
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
        //// Draw the raycast in the scene view for debugging (it was aray cast before a sphere)
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, transform.forward * stopDistance);

        // Draw the sphere cast for car detection in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * stopDistance, carFront);

        // Draw the sphere cast in the scene view for debugging
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * playerDistance, carFront);
    }
}

