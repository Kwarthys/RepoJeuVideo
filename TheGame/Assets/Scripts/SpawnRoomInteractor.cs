using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomInteractor : AbstractInteract
{
    private GameObject roomPrefab;

    [SerializeField] private Transform exitPoint;

    private bool spawned = false;

    private static int counter = 1;

    public void Start()
    {
        roomPrefab = Resources.Load<GameObject>("QuadRoom");
    }

    public override void execute()
    {
        if(!spawned)
        {
            gameObject.SetActive(false);
            Debug.Log(gameObject);
            Debug.Log(roomPrefab);

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
