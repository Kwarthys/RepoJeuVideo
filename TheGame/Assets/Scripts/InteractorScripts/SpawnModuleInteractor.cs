using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnModuleInteractor : AbstractInteract
{
    public GameObject modulePrefab;

    private Transform spawnPoint;

    public void Start()
    {
        spawnPoint = transform.Find("SpawnPoint");
    }

    public override void execute()
    {
        gameObject.SetActive(false);

        Instantiate(modulePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
