using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : RessourceReceiver
{
    public int containerSpace;

    private int incomingDeliveries = 0;

    private Dictionary<RessourceManager.Ressources, int> amounts = new Dictionary<RessourceManager.Ressources, int>();
    private Dictionary<RessourceManager.Ressources, int> bookedCrates = new Dictionary<RessourceManager.Ressources, int>();

    private RessourceManager rManager;
    private void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        rManager.registerContainer(this);
    }

    public override void notifyDelivery(CrateBehavior crate)
    {
        --incomingDeliveries;
        
        if(amounts.ContainsKey(crate.r))
        {
            ++amounts[crate.r];
        }
        else
        {
            amounts[crate.r] = 1;
        }
    }

    public int getLoad()
    {
        int a = 0;
        foreach (int v in amounts.Values)
        {
            a += v;
        }
        return a;
    }

    public bool hasResource(RessourceManager.Ressources r)
    {
        if(amounts.ContainsKey(r))
        {
            int amount = amounts[r] - (bookedCrates.ContainsKey(r) ? bookedCrates[r] : 0);
            return amounts[r] != 0;
        }

        return false;
    }

    public void takeACrate(RessourceManager.Ressources r)
    {
        --amounts[r];
        --bookedCrates[r];
    }

    public void notifyIncomingDelivry()
    {
        ++incomingDeliveries;
    }

    public bool isFull()
    {
        int a = incomingDeliveries;
        foreach(int v in amounts.Values)
        {
            a += v;
        }

        return a >= containerSpace;
    }

    public override void notifyPickUp(RessourceManager.Ressources r)
    {
        takeACrate(r);
    }

    internal void bookCrate(RessourceManager.Ressources r)
    {
        if(bookedCrates.ContainsKey(r))
        {
            ++bookedCrates[r];
        }
        else
        {
            bookedCrates.Add(r, 1);
        }
    }
}
