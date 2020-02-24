using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    private enum BOTSTATE {idle, toCrate, toModule};

    private Vector3 target;
    private bool enroute = false;
    private NavMeshAgent agent;

    private RessourceManager rManager;

    private BOTSTATE state = BOTSTATE.idle;

    private Order order;
    private bool busy = false;
    public bool isBusy() { return busy; }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        rManager.registerBot(this);
    }

    public void setOrder(Order o)
    {
        order = o;
        busy = true;
        Debug.Log(o.crate);
        agent.SetDestination(o.crate.transform.position);
        state = BOTSTATE.toCrate;
    }

    
    void Update()
    {
        if(state == BOTSTATE.toCrate)
        {
            if(agent.remainingDistance < 0.2f)
            {
                //GRAB BOX
                state = BOTSTATE.toModule;
                agent.SetDestination(order.module.transform.position);
            }
        }
        else if(state == BOTSTATE.toModule)
        {
            if (agent.remainingDistance < 0.2f)
            {
                //DROP BOX
                state = BOTSTATE.idle;
                busy = false;
                //GO TO BASE 
                agent.SetDestination(Vector3.zero);
            }
        }
    }
    
}
