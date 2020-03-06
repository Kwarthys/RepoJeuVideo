using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehavior : MonoBehaviour
{
    public RessourceManager.Ressources r;

    public void setType(RessourceManager.Ressources r)
    {
        this.r = r;

        Color c = Color.red;

        switch(r)
        {
            case RessourceManager.Ressources.Iron:
                c = Color.black;
                break;

            case RessourceManager.Ressources.RawIron:
                c = Color.grey;
                break;
        }

        GetComponent<Renderer>().material.color = c;
    }
}
