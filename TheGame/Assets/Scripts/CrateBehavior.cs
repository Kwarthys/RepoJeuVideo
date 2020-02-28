using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehavior : MonoBehaviour
{
    private RessourceManager manager;

    public RessourceManager.Ressources r;

    void Start()
    {
        manager = GameObject.FindWithTag("RessourceManager").GetComponent<RessourceManager>();
        manager.notifyCrateSpawn(this);
        //Debug.Log("notified " + r);
    }
}
