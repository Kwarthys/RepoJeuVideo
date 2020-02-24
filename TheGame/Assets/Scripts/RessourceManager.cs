using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Order
{
    public RessourceProcessor module;
    public RessourceManager.Ressources r;
    public CrateBehavior crate;

    public bool running = false;

    public Order(RessourceProcessor module, RessourceManager.Ressources r)
    {
        this.module = module;
        this.r = r;
    }
}

public class RessourceManager : MonoBehaviour
{
    public enum Ressources {RawIron, Iron};

    public Text text;

    private Dictionary<Ressources, int> amounts = new Dictionary<Ressources, int>();

    public int ressourceAmount = 10;

    private List<CrateBehavior> crates = new List<CrateBehavior>();
    private List<Order> orders = new List<Order>();
    private List<RobotController> bots = new List<RobotController>();

    private void Start()
    {
        foreach(Ressources r in Enum.GetValues(typeof(Ressources)))
        {
            amounts[r] = ressourceAmount;
        }
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
            int rand = (int)Random.Range(0, orders.Count-1);
            Order o = orders[rand];

            if(amounts[o.r] > 0)
            {
                List<RobotController> readyBots = getAvailableBots();

                if(readyBots.Count > 0)
                {
                    //GET A CRATE
                    CrateBehavior c = getCrateOf(o.r);
                    if(c != null)
                    {
                        o.crate = c;
                        crates.Remove(c);
                        --amounts[c.r];
                        //ASSIGN ORDER TO readyBots[0]
                        readyBots[0].setOrder(o);
                        o.running = true;
                        orders.Remove(o);
                    }
                }
            }
        }
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

        foreach(RobotController bot in bots)
        {
            if(!bot.isBusy())
            {
                readyBots.Add(bot);
            }
        }

        return readyBots;
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
}
