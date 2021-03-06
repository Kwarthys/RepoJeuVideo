﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorDisplayCanvasInteractor : AbstractInteract
{
    [SerializeField] private LayerMask layerMaskInteract;

    public Transform exitPoint;

    private GameObject canvas;

    private bool canvasShowed = false;

    private List<SpawnRoomInteractor> children = new List<SpawnRoomInteractor>();

    private NavMeshSurface surface;

    public void Start()
    {
        Vector3 fwd = exitPoint.TransformDirection(Vector3.forward);
        surface = GameObject.FindWithTag("NavMeshManager").GetComponent<NavMeshSurface>();

        if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, 1, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Object"))
            {
                hit.collider.gameObject.SetActive(false);
                gameObject.SetActive(false);

                surface.BuildNavMesh();
            }
        }
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);

        setupChildSpawner("ImageSmallRoom");
        setupChildSpawner("ImageBigRoom");
        setupChildSpawner("ImageHangar");
    }

    private void setupChildSpawner(string name)
    {
        Transform child = transform.Find("Canvas").Find(name);
        SpawnRoomInteractor childInteractor = child.GetComponent<SpawnRoomInteractor>();
        childInteractor.exitPoint = exitPoint;
        childInteractor.cb = callback;

        children.Add(childInteractor);
    }

    private void checkChildrenCollisions()
    {
        foreach(SpawnRoomInteractor child in children)
        {
            bool collides = child.wouldItCollide();
            child.gameObject.SetActive(!collides);
        }
    }

    private void callback()
    {
        gameObject.SetActive(false);

        surface.BuildNavMesh();
    }

    public override void execute()
    {
        canvasShowed = !canvasShowed;
        canvas.SetActive(canvasShowed);

        if(canvasShowed)
        {
            checkChildrenCollisions();
        }
    }
}
