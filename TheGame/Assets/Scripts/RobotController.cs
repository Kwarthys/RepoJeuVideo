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

    private int staticTime = 0;

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
        //Debug.Log(transform.name + " received order" + o.id + " hasCrate?" + (o.crate != null) + " hasPickUp?" + (o.pickUpModule!=null));
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
        if(isBusy() && agent.velocity.magnitude == 0)
        {
            if(++staticTime > 200)
            {
                agent.SetDestination(agent.destination);
                staticTime = 0;
            }
        }


        if(state == BOTSTATE.toCrate)
        {
            if(agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                //GRAB BOX
                //Debug.Log("Accessing crate for order" + order.id);
                //Debug.Log(transform.name + " Accessing " + order.crate.GetInstanceID() + " of " + order.crate.r + " for order" + order.id);
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
                state = BOTSTATE.idle;
                order.deliveryModule.notifyDelivery(order.crate);
                //GO TO BASE 
                agent.SetDestination(Vector3.zero);

                //Destro BOX Model
                //Debug.Log(transform.name + " Destroying " + order.crate.GetInstanceID() + " of " + order.crate.r + " for order" + order.id);
                Destroy(order.crate.gameObject);
            }
        }
        else if(state == BOTSTATE.toPickUp)
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                order.pickUpModule.notifyPickUp(order.r);
                //GET BOX
                GameObject crate = Instantiate(rManager.cratePrefab, transform);
                crate.transform.localPosition = new Vector3(0, 1f, -0.2f);
                CrateBehavior c = crate.transform.GetComponent<CrateBehavior>();
                order.crate = c;
                c.setType(order.r);
                //Debug.Log(transform.name + " Spawning " + order.crate.GetInstanceID() + " of " + order.crate.r + " for order" + order.id);

                state = BOTSTATE.toModule;
                agent.SetDestination(order.deliveryModule.transform.position);
            }
        }
    }
    
}
