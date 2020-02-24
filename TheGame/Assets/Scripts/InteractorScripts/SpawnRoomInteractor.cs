using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CollisionDetector;

public class SpawnRoomInteractor : AbstractInteract
{
    private GameObject roomPrefab;
    public Transform exitPoint;
    private bool spawned = false;

    public string RessourceRoomName;

    private static int counter = 1;

    public delegate void CallBack();
    public CallBack cb;

    public void Start()
    {
        roomPrefab = Resources.Load<GameObject>(RessourceRoomName);
    }

    public override void execute()
    {
        if(!spawned)
        {
            spawned = true;            

            Transform newRoomEntryPoint = roomPrefab.transform.Find("Points/EntryPoint");

            GameObject roomSpawned = Instantiate(roomPrefab, new Vector3(1000,1000,1000), Quaternion.LookRotation(exitPoint.forward, exitPoint.up) * Quaternion.Inverse(newRoomEntryPoint.rotation));
            roomSpawned.name = "room" + counter++;

            newRoomEntryPoint = roomSpawned.transform.Find("Points/EntryPoint");

            Vector3 worldPosExitPoint = exitPoint.position;
            Vector3 worldPosEntryPoint = newRoomEntryPoint.position;

            Vector3 spawnPos = worldPosExitPoint;
            spawnPos -= worldPosEntryPoint;

            roomSpawned.transform.position += spawnPos;

            if(cb != null)
            {
                cb();
            }
        }
        else
        {
            Debug.Log("Already spawned the room");
        }
    }

    public bool wouldItCollide()
    {
        bool collides = false;
        CollisionSphereData[] ds = CollisionDetector.getSpheresFor(RessourceRoomName, exitPoint);
        foreach(CollisionSphereData d in ds)
        {
            collides = collides || Physics.SphereCast(d.origin, d.radius, d.direction, out RaycastHit hit, d.maxDistance);
            Debug.DrawLine(d.origin, d.direction * d.maxDistance + d.origin, RessourceRoomName == "QuadRoom" ? Color.black : Color.white, 10);
        }
        return collides;
    }
}
