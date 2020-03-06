using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public RessourceManager.Ressources input;
    public int nbInput;
    public RessourceManager.Ressources output;
    public int nbOutput;

    public int rawTimeToProcess;

    public Recipe(RessourceManager.Ressources input, int nbInput, RessourceManager.Ressources output, int nbOutput, int rawProcessTime)
    {
        this.input = input;
        this.nbInput = nbInput;
        this.output = output;
        this.nbOutput = nbOutput;
        this.rawTimeToProcess = rawProcessTime;
    }

    public string getText()
    {
        return nbInput + "x " + input + " -> " + nbOutput + "x " + output;
    }
}
