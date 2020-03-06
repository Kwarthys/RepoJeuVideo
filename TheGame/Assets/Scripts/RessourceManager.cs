using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Order
{
    public RessourceReceiver deliveryModule;
    public RessourceReceiver pickUpModule;
    public RessourceManager.Ressources r;
    public CrateBehavior crate;

    public bool running = false;

    public Order(RessourceReceiver module, RessourceManager.Ressources r)
    {
        this.deliveryModule = module;
        this.r = r;
    }
}

public class RessourceManager : MonoBehaviour
{
    public enum Ressources {RawIron, Iron};

    public GameObject cratePrefab;

    public Text text;

    private Dictionary<Ressources, int> amounts = new Dictionary<Ressources, int>();

    public int ressourceAmount = 10;

    private bool debug = false;

    private List<CrateBehavior> crates = new List<CrateBehavior>();
    private List<Order> orders = new List<Order>();
    private List<RobotController> bots = new List<RobotController>();
    private List<ContainerController> containers = new List<ContainerController>();

    private void Start()
    {
        foreach(Ressources r in Enum.GetValues(typeof(Ressources)))
        {
            amounts[r] = ressourceAmount;
        }
    }

    public void postPushResources(Ressources r, RessourceReceiver pickUpModule)
    {
        Order o = new Order(null, r);
        o.pickUpModule = pickUpModule;
        orders.Add(o);

        ++amounts[r];
    }

    public void postOrder(Order o)
    {
        orders.Add(o);
    }

    private void updateText()
    {
        text.text = "";
        foreach (KeyValuePair<Ressources, int> kv in amounts)
        {
            text.text += kv.Key + ": " + kv.Value + "\n";
        }
    }

    private void Update()
    {
        updateText();
        tryAnOrder();
    }

    private void tryAnOrder()
    {
        if(orders.Count > 0)
        {
            int rand = (int)Random.Range(0, orders.Count);
            Order o = orders[rand];

            if(o.deliveryModule == null)
            {
                bool foundAnOrder = false;
                //IT IS A PUSH, we must find an order for that resource, or a container.
                for(int i = 0; i < orders.Count && !foundAnOrder; ++i)
                {
                    if(orders[i].r == o.r && orders[i].deliveryModule != null) // if order is about the same resource AND not a push
                    {
                        o.deliveryModule = orders[i].deliveryModule;            //Merging the orders and deleting one.
                        orders.Remove(orders[i]);
                        foundAnOrder = true;
                    }
                }

                if(!foundAnOrder)
                {
                    //We must find a container
                    ContainerController cc = getRandomAvailableContainer();
                    if(cc != null)
                    {
                        o.deliveryModule = cc;
                        cc.notifyIncomingDelivry();
                    }
                    else
                    {
                        return; //Can't find any order or containers, can't do anything.
                    }
                }
            }

            if (amounts[o.r] > 0)
            {
                List<RobotController> readyBots = getAvailableBots();

                if(readyBots.Count > 0)
                {
                    if(o.pickUpModule != null)
                    {
                        //ITS A PUSH
                        getRandomAvailableBot().setOrder(o);
                        orders.Remove(o);
                    }
                    else
                    {
                        //GET A CRATE
                        CrateBehavior c = getCrateOf(o.r);
                        if (c != null)
                        {
                            o.crate = c;
                            crates.Remove(c);
                            --amounts[c.r];
                            //ASSIGN ORDER
                            getRandomAvailableBot().setOrder(o);
                            orders.Remove(o);
                        }
                        else
                        {
                            ContainerController cc = getContainerWith(o.r);
                            if (cc != null)
                            {
                                o.pickUpModule = cc;
                                --amounts[c.r];
                                //ASSIGN ORDER
                                getRandomAvailableBot().setOrder(o);
                                orders.Remove(o);
                            }
                        }
                    }
                }
                else if (debug)
                {
                    Debug.Log("Not enough bots for " + o.r + " order.");
                }
            }
            else if(debug)
            {
                Debug.Log("Not enough resrouces for " + o.r + " order.");
            }
        }
        else if (debug)
        {
            Debug.Log("No order.");
        }
    }

    private ContainerController getContainerWith(Ressources r)
    {
        foreach (ContainerController cc in containers)
        {
            if (cc.hasResource(r))
            {
                return cc;
            }
        }
        return null;
    }

    private CrateBehavior getCrateOf(Ressources r)
    {
        foreach(CrateBehavior c in crates)
        {
            if(c.r == r)
            {
                return c;
            }
        }

        return null;
    }

    private List<RobotController> getAvailableBots()
    {
        List<RobotController> readyBots = new List<RobotController>();

        foreach (RobotController bot in bots)
        {
            if (!bot.isBusy())
            {
                readyBots.Add(bot);
            }
        }

        return readyBots;
    }

    private RobotController getRandomAvailableBot()
    {
        List<RobotController> availableBots = getAvailableBots();
        if (availableBots.Count == 0)
        {
            return null;
        }

        return availableBots[Random.Range(0, availableBots.Count - 1)];
    }

    private ContainerController getRandomAvailableContainer()
    {
        List<ContainerController> availableContainers = getAvailableContainers();
        if (availableContainers.Count == 0)
        {
            return null;
        }

        return availableContainers[Random.Range(0, availableContainers.Count - 1)];
    }


    private List<ContainerController> getAvailableContainers()
    {
        List<ContainerController> availableContainers = new List<ContainerController>();

        foreach (ContainerController c in containers)
        {
            if (!c.isFull())
            {
                availableContainers.Add(c);
            }
        }

        return availableContainers;
    }

    public int getAmountOf(Ressources r)
    {
        return amounts[r];
    }

    public bool takeRessource(Ressources r, int amount)
    {
        if(amounts[r] >= amount)
        {
            amounts[r] -= amount;
            //Debug.Log("Gave " + amount + " " + r);
            return true;
        }

        return false;
    }

    public void giveRessources(Ressources r, int amount)
    {
        amounts[r] += amount;
        //Debug.Log("Received " + amount + " " + r);
    }



    public void notifyCrateSpawn(CrateBehavior crate)
    {
        amounts[crate.r] += 1;
        crates.Add(crate);
    }

    public void registerBot(RobotController bot)
    {
        bots.Add(bot);
    }

    public void registerContainer(ContainerController container)
    {
        containers.Add(container);
    }
}
