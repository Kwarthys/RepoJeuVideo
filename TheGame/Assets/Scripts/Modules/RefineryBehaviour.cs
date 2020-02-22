using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryBehaviour : MonoBehaviour
{
    public int timeToProcess;
    private int processIndex = 0;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject smoke;

    private bool loaded = false;

    private RessourceManager rManager;

    void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!loaded)
        {
            loaded = checkCondition();
            toggleActive(loaded);
        }
        else
        {
            slider.value = processIndex * 1.0f / timeToProcess;
            if(++processIndex >= timeToProcess)
            {
                rManager.giveRessources(RessourceManager.Ressources.Iron, 1);
                processIndex = 0;

                loaded = checkCondition();
                toggleActive(loaded);
            }
        }
    }

    private bool checkCondition()
    {
        return rManager.takeRessource(RessourceManager.Ressources.RawIron, 2);
    }

    private void toggleActive(bool command)
    {
        canvas.SetActive(command);
        smoke.SetActive(command);
    }
}
