using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoomInteractor : AbstractInteract
{
    [SerializeField] private GameObject room;

    private bool spawned = false;

    public override void execute()
    {
        if(!spawned)
        {
            spawned = true;
            
            Vector3 spawnPos = transform.parent.Find("ExitPoint").position;
            spawnPos -= room.transform.Find("EntryPoint").localPosition;
            Debug.Log(spawnPos + " :" + transform.parent.Find("ExitPoint").position + " " + room.transform.Find("EntryPoint").position);
            GameObject roomSpawned = Instantiate(room, spawnPos, Quaternion.identity);

        }
        else
        {
            Debug.Log("Already spawned the room");
        }
    }
}
