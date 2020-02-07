using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomInteractor : AbstractInteract
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Transform exitPoint;

    private bool spawned = false;

    private static int counter = 1;

    public override void execute()
    {
        if(!spawned)
        {
            transform.localScale = Vector3.zero;
            Debug.Log(gameObject);

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
