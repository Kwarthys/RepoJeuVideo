using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RessourceReceiver : MonoBehaviour
{
    public abstract void notifyDelivery(CrateBehavior crate);
    public abstract void notifyPickUp(RessourceManager.Ressources r);
}
