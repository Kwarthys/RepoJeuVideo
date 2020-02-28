using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    private enum BOTSTATE {idle, toCrate, toPickUp, toModule};

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
        if(o.crate != null)
        {
            agent.SetDestination(o.crate.transform.position);
            state = BOTSTATE.toCrate;
        }
        else
        {
            agent.SetDestination(o.pickUpModule.transform.position);
            state = BOTSTATE.toPickUp;
        }
    }

    
    void Update()
    {
        if(state == BOTSTATE.toCrate)
        {
            if(agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                //GRAB BOX
                order.crate.transform.SetParent(transform);
                order.crate.transform.localPosition = new Vector3(0, 1f, -0.2f);
                order.crate.transform.localRotation = Quaternion.identity;
                state = BOTSTATE.toModule;
                agent.SetDestination(order.deliveryModule.transform.position);
            }
        }
        else if(state == BOTSTATE.toModule)
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                //DROP BOX
                Destroy(order.crate.gameObject);

                state = BOTSTATE.idle;
                order.deliveryModule.notifyDelivery(order.crate);
                //GO TO BASE 
                agent.SetDestination(Vector3.zero);
            }
        }
        else if(state == BOTSTATE.toPickUp)
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                order.pickUpModule.notifyPickUp();
                //GET BOX
                GameObject crate = Instantiate(rManager.cratePrefab, transform);
                crate.transform.localPosition = new Vector3(0, 1f, -0.2f);
                CrateBehavior c = crate.transform.GetComponent<CrateBehavior>();
                order.crate = c;
                c.r = order.r;

                state = BOTSTATE.toModule;
                agent.SetDestination(order.deliveryModule.transform.position);
            }
        }
    }
    
}
