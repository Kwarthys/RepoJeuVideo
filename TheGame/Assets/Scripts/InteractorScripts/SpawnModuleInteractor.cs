using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModuleInteractor : AbstractInteract
{
    public GameObject modulePrefab;

    public GameObject spawnerRoot;

    public Transform spawnPoint;

    public override void execute()
    {
        spawnerRoot.SetActive(false);

        Instantiate(modulePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
