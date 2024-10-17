using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    // Speed of the individual flock (boid/fish)
    float speed;

    // Boolean flag to check if the fish needs to turn (when hitting boundaries)
    bool turning = false;

    // Start is called before the first frame update
    // Initializes the fish with a random speed within the bounds set by FlockManager
    void Start()
    {
        // Set initial speed randomly between minimum and maximum speed set in the FlockManager
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    // Update is called once per frame
    // Handles fish movement, boundary checks, and rule application (cohesion, separation, alignment)
    void Update()
    {
        // Define the swimming area boundary (based on the swimLimits set in FlockManager)
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);

        // If the fish is outside the boundary, set turning to true to make it turn back
        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        // If the fish needs to turn (because it hit a boundary)
        if (turning)
        {
            // Calculate the direction towards the center of the swim area
            Vector3 direction = FlockManager.FM.transform.position - transform.position;

            // Smoothly rotate towards the center using Slerp (Spherical Linear Interpolation)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Randomly adjust the fish's speed occasionally (10% chance per frame)
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }

            // Randomly apply flocking rules occasionally (10% chance per frame)
            if (Random.Range(0, 100) < 10)
            {
                ApplyFlockingRules();
            }
        }

        // Move the fish forward based on its current speed
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // ApplyFlockingRules() is responsible for implementing the core flocking behaviors:
    // - Cohesion: Stay near the center of the group.
    // - Separation: Avoid getting too close to other fish.
    // - Alignment: Match the speed of nearby fish.
    void ApplyFlockingRules()
    {
        // Retrieve the list of all fish in the flock from the FlockManager
        GameObject[] gos = FlockManager.FM.allFish;

        // Variables for calculating flocking behavior
        Vector3 vcentre = Vector3.zero;  // Center position of the flock
        Vector3 vavoid = Vector3.zero;   // Avoidance vector for nearby fish
        float gSpeed = 0.01f;            // Average speed of the flock
        float nDistance;                 // Distance to another fish
        int groupSize = 0;               // Number of nearby fish (for calculating averages)

        // Loop through all fish in the flock
        foreach (GameObject go in gos)
        {
            // Exclude the current fish from the calculation
            if (go != this.gameObject)
            {
                // Calculate the distance between the current fish and the fish being examined
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                // If the fish is within the neighbor distance (defined in FlockManager)
                if (nDistance < FlockManager.FM.neighbourDistance)
                {
                    // Cohesion: Add the position of the nearby fish to the center vector
                    vcentre += go.transform.position;
                    groupSize++;

                    // Separation: If the fish is too close, apply an avoidance force
                    if (nDistance < 1.0f)
                    {
                        vavoid += this.transform.position - go.transform.position;
                    }

                    // Alignment: Match speed with nearby fish
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        // If there are nearby fish, adjust position and speed
        if (groupSize > 0)
        {
            // Cohesion: Calculate the average position of the group and move toward it
            vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);

            // Alignment: Set the speed to the average speed of the group
            speed = gSpeed / groupSize;

            // Ensure the speed does not exceed the maximum limit set in FlockManager
            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            // Calculate the direction to move based on cohesion and avoidance
            Vector3 direction = (vcentre + vavoid) - this.transform.position;

            // If there's a direction to move, smoothly rotate toward it using Slerp
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
