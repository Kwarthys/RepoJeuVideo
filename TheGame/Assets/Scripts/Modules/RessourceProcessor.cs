using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceProcessor : MonoBehaviour
{
    private enum STATE { idle, processing, waiting};
    public RessourceManager.Ressources input;
    public int nbInput;
    public RessourceManager.Ressources output;
    public int nbOutput;
    public int timeToProcess;

    private STATE state = STATE.idle;

    private int processIndex = 0;
    public float getCompletion() { return processIndex * 1.0f / timeToProcess; }

    private bool proccessing = false;

    private RessourceManager rManager;

    private int loadedInputR = 0;

    public void notifyDelivery()
    {
        loadedInputR++;
    }

    void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case STATE.idle:
                //MAKE ORDERS
                for(int i = 0; i < nbInput; ++i)
                {
                    rManager.postOrder(new Order(this, input));
                }
                state = STATE.waiting;
                break;

            case STATE.processing:
                if(processIndex++ >= timeToProcess)
                {
                    state = STATE.idle;
                    rManager.giveRessources(output, nbOutput); //CHANGE THAT TO CRATE SPAWN
                    processIndex = 0;
                }
                break;

            case STATE.waiting:
                if(loadedInputR == nbInput)
                {
                    loadedInputR = 0;
                    state = STATE.processing;
                }
                break;
        }
    }
}
