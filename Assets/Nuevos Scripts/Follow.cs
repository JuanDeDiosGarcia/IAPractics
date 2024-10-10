using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Material mat1;

    public GameObject target;

    public GameObject search;


    public float movSpeed = 10.0f;
    public float maxSpeed = 1.0f;

    public float acceleration = 1.0f;

    public float turnSpeed = 0.1f;
    public float turnAcceleration = 1.0f;
    public float maxTurnSpeed = 1.0f;

    Quaternion rotation;

    public float stopDistance = 1.0f;


    private void Start()
    {
        mat1.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        search.transform.localScale = new Vector3(stopDistance*2,1,stopDistance*2);

        if (Vector3.Distance(target.transform.position, transform.position) <
               stopDistance)
        {
            mat1.color = Color.blue;
            GetComponent<Renderer>().material = mat1;
            return;
        }
        else
            mat1.color = Color.white;



        Seek();   // calls to this function should be reduced

        turnSpeed += turnAcceleration * Time.deltaTime;
        turnSpeed = Mathf.Min(turnSpeed, maxTurnSpeed);

        movSpeed += acceleration * Time.deltaTime;
        movSpeed = Mathf.Min(movSpeed, maxSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              rotation, Time.deltaTime * turnSpeed);

        transform.position += transform.forward.normalized * movSpeed *
                              Time.deltaTime;


    }


    void Seek()
    {
        Vector3 movement;

        Vector3 direction = target.transform.position - transform.position;

        direction.y = 0f;

        movement = direction.normalized * acceleration;

        float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);

        rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
