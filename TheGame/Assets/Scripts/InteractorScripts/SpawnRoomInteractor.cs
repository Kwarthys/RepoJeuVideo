using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomInteractor : AbstractInteract
{
    private GameObject roomPrefab;

    [SerializeField] private Transform exitPoint;
    [SerializeField] private LayerMask layerMaskInteract;

    private bool spawned = false;

    private static int counter = 1;

    public void Start()
    {
        RaycastHit hit;
        Vector3 fwd = exitPoint.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, out hit, 1, layerMaskInteract.value))
        {
            if (hit.collider.CompareTag("Object"))
            {
                hit.collider.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else
        {
            roomPrefab = Resources.Load<GameObject>("QuadRoom");
        }
    }

    public override void execute()
    {
        if(!spawned)
        {
            gameObject.SetActive(false);

            spawned = true;
            
            Vector3 spawnPos = exitPoint.position;

            spawnPos += exitPoint.forward * roomPrefab.transform.Find("Points/EntryPoint").localPosition.magnitude;

            GameObject roomSpawned = Instantiate(roomPrefab, spawnPos, exitPoint.rotation);
            roomSpawned.name = "room" + counter;
            counter++;

        }
        else
        {
            Debug.Log("Already spawned the room");
        }
    }
}
