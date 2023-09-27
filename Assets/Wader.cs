using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Wader : MonoBehaviour
{

    float radius = 50;
    float offset;

    Vector3 localTarget;
    Vector3 worldTarget;

    public bool enCamino = false;

    NavMeshAgent agent;

    public GameObject wanderPoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if(enCamino == false)
        {
           randomePosition();

        }

        // worldTarget = transform.TransformPoint(localTarget);
        //worldTarget.y = wanderPoint.transform.position.y;



        agent.destination = localTarget;

      if (Vector3.Distance(transform.position, agent.destination) <=1)
        { 
            enCamino = false;
        }
        


    }

    void randomePosition()
    {

        localTarget = UnityEngine.Random.insideUnitCircle * radius;
        localTarget.z = localTarget.y;

        Debug.Log(localTarget);

        localTarget += wanderPoint.transform.position;
        localTarget.y = 0;


        enCamino = true;


    }
}
