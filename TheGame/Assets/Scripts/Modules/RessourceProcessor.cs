using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceProcessor : RessourceReceiver
{
    private enum STATE { idle, processing, waitingDelivery, waitingPickUp};
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
    private int loadOutput = 0;

    public override void notifyDelivery(CrateBehavior crate)
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
                state = STATE.waitingDelivery;
                break;

            case STATE.processing:
                if(processIndex++ >= timeToProcess)
                {
                    state = STATE.waitingPickUp;
                    //rManager.giveRessources(output, nbOutput); //CHANGE THAT TO CRATE SPAWN
                    for(int i = 0; i < nbOutput; ++i)
                    {
                        rManager.postPushResources(output, this);
                    }
                    loadOutput = nbOutput;
                    processIndex = 0;
                }
                break;

            case STATE.waitingDelivery:
                if(loadedInputR == nbInput)
                {
                    loadedInputR = 0;
                    state = STATE.processing;
                }
                break;

            case STATE.waitingPickUp:
                if(loadOutput == 0)
                {
                    state = STATE.idle;
                }
                break;
        }
    }

    public override void notifyPickUp()
    {
        --loadOutput;
    }
}
