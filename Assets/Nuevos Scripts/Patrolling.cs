using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Patrolling : MonoBehaviour
{
    public NavMeshAgent patrollingAgent;
    public Transform ghostAgent;

    private void Update()
    {
        patrollingAgent.SetDestination(ghostAgent.position);
    }
}
