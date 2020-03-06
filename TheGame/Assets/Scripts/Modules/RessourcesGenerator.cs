using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesGenerator : MonoBehaviour
{
    private RessourceManager rManager;

    private GameObject cratePrefab;
    public RessourceManager.Ressources generatedResource;
    public int timeToProcess;
    public int generatedAmount;

    public Transform holderAndRef;

    private int processIndex = 0;

    void Start()
    {
        rManager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        cratePrefab = rManager.cratePrefab;
    }


    void Update()
    {
        if(holderAndRef.childCount == 0)
        {
            if (++processIndex >= timeToProcess)
            {
                //SPAWN
                Vector3 offset = new Vector3(0, 0.75f, 0);
                for(int i = 0; i < generatedAmount; ++i)
                {
                    rManager.postPushCrate(spawnCrate(offset));
                    offset += 1.2f * holderAndRef.forward;
                }
                processIndex = 0;
            }
        }
    }

    private CrateBehavior spawnCrate(Vector3 offset)
    {
        GameObject g = Instantiate(cratePrefab, holderAndRef.position + offset, Quaternion.identity, holderAndRef);
        CrateBehavior c = g.GetComponent<CrateBehavior>();
        c.setType(generatedResource);

        return c;
    }
}
