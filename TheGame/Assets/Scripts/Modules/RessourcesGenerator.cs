using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesGenerator : MonoBehaviour
{
    private RessourceManager rManager;

    public RessourceManager.Ressources generatedResource;
    public int timeToProcess;
    public int generatedAmount;

    private int processIndex = 0;

    void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
    }


    void Update()
    {
        if (++processIndex >= timeToProcess)
        {
            rManager.giveRessources(generatedResource, generatedAmount);
            processIndex = 0;
        }
    }
}
