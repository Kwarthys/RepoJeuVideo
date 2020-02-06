using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCaster : MonoBehaviour
{
    private GameObject raycastedObject;

    [SerializeField] private int rayLenght = 3;
    [SerializeField] private LayerMask layerMaskInteract;

    [SerializeField] private Image uiCrossHair;

    private bool crossHairActivated = false;

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(transform.position, fwd, out hit, rayLenght, layerMaskInteract.value))
        {
            if(hit.collider.CompareTag("Object"))
            {
                raycastedObject = hit.collider.gameObject;

                if(!crossHairActivated)
                {
                    crossHairActive(true);
                }
                        

                if (Input.GetKeyDown("e"))
                {
                    raycastedObject.GetComponent<AbstractInteract>().execute();
                }
            }
        }
        else
        {
            if(crossHairActivated)
            {
                crossHairActive(false);
            }
        }

    }

    void crossHairActive(bool status)
    {
        crossHairActivated = status;
        uiCrossHair.color = status ? Color.red : Color.white;
    }

}
