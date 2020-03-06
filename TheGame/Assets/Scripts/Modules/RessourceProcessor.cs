using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceProcessor : RessourceReceiver
{
    private enum STATE { idle, processing, waitingDelivery, waitingPickUp};

    private Recipe recipe;
    private Recipe switchingRecipe;

    private STATE state = STATE.idle;

    private int processIndex = 0;
    public float getCompletion()
    {
        if (recipe == null)
            return -1;
        return processIndex * 1.0f / recipe.rawTimeToProcess;
    }

    private bool proccessing = false;

    private RessourceManager rManager;

    private int loadedInputR = 0;
    private int loadOutput = 0;

    private CanvasToogler ct;

    public override void notifyDelivery(CrateBehavior crate)
    {
        loadedInputR++;
    }

    void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        ct = GetComponent<CanvasToogler>();
    }

    // Update is called once per frame
    void Update()
    {

        if(state == STATE.idle && switchingRecipe != null)
        {
            recipe = switchingRecipe;
            switchingRecipe = null;
        }

        if (recipe == null)
        {
            ct.show();
            return;
        }

        switch (state)
        {
            case STATE.idle:
                //MAKE ORDERS
                for(int i = 0; i < recipe.nbInput; ++i)
                {
                    rManager.postOrder(new Order(this, recipe.input));
                }
                //Debug.Log(transform.name + " posted " + recipe.input + " order(s).");
                state = STATE.waitingDelivery;
                break;

            case STATE.processing:
                if(processIndex++ >= recipe.rawTimeToProcess)
                {
                    state = STATE.waitingPickUp;
                    for(int i = 0; i < recipe.nbOutput; ++i)
                    {
                        rManager.postPushResources(recipe.output, this);
                    }
                    loadOutput = recipe.nbOutput;
                    processIndex = 0;
                }
                break;

            case STATE.waitingDelivery:
                if(loadedInputR == recipe.nbInput)
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

    public override void notifyPickUp(RessourceManager.Ressources r)
    {
        --loadOutput;
    }

    public void setRecipe(Recipe r)
    {
        this.switchingRecipe = r;
        ct.hide();
    }
}
