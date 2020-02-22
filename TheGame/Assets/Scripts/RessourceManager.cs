using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RessourceManager : MonoBehaviour
{
    public enum Ressources {RawIron, Iron};

    public Text text;

    private Dictionary<Ressources, int> amounts = new Dictionary<Ressources, int>();

    public int ressourceAmount = 10;

    private void Start()
    {
        foreach(Ressources r in Enum.GetValues(typeof(Ressources)))
        {
            amounts[r] = ressourceAmount;
        }
    }

    public void updateText()
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
}
