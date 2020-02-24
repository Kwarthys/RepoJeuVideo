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
    public bool isBusy() { return state!=BOTSTATE.idle; }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        rManager.registerBot(this);
    }

    public void setOrder(Order o)
    {
        order = o;
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
                order.crate.transform.SetParent(transform);
                order.crate.transform.localPosition = new Vector3(0, 1f, -0.2f);
                order.crate.transform.localRotation = Quaternion.identity;
                agent.SetDestination(order.module.transform.position);
                state = BOTSTATE.toModule;
            }
        }
        else if(state == BOTSTATE.toModule)
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                //DROP BOX
                Destroy(order.crate.gameObject);

                state = BOTSTATE.idle;
                order.module.notifyDelivery();
                //GO TO BASE 
                agent.SetDestination(Vector3.zero);
            }
        }
    }
    
}
