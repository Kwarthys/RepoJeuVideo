using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDisplayCanvasInteractor : AbstractInteract
{
    [SerializeField] private LayerMask layerMaskInteract;

    public Transform exitPoint;

    private GameObject canvas;

    private bool canvasShowed = false;

    public void Start()
    {
        Vector3 fwd = exitPoint.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, 1, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Object"))
            {
                hit.collider.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);

        setupChildSpawner("ImageSmallRoom");
        setupChildSpawner("ImageBigRoom");
    }

    private void setupChildSpawner(string name)
    {
        Transform child = transform.Find("Canvas").Find(name);
        child.GetComponent<SpawnRoomInteractor>().exitPoint = exitPoint;
        child.GetComponent<SpawnRoomInteractor>().cb = callback;
    }

    private void callback()
    {
        gameObject.SetActive(false);
    }

    public override void execute()
    {
        canvasShowed = !canvasShowed;
        canvas.SetActive(canvasShowed);
    }
}
