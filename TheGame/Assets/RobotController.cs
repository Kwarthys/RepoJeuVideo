using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    private Vector3 target;
    private bool enroute = false;
    private NavMeshAgent agent;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(!enroute)
        {
            target = new Vector3(Random.Range(-6, 6), 0.5f, Random.Range(-12, 12));
            agent.SetDestination(target);

            enroute = true;
        }
        
        if(agent.remainingDistance < 0.3f)
        {
            enroute = false;
        }
        
    }
}
