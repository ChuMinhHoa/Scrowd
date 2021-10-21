using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    [SerializeField] GameObject normalE;
    [SerializeField] float spawnTime, spawnTimeSetting;
    public List<GameObject> normalESpawneds;
    [SerializeField] int countE; 

    private void FixedUpdate()
    {
        if (spawnTime <= 0f && normalESpawneds.Count <= countE)
        {
            SpawnNow();
            spawnTime = spawnTimeSetting;
        }
        else if (normalESpawneds.Count <= 0)
            spawnTime -= Time.deltaTime;

    }

    void SpawnNow() {

        for (int i = 0; i < countE; i++)
        {
            GameObject spawned = Instantiate(normalE, transform.position, Quaternion.identity,transform);
            spawned.GetComponent<Partner>().mySpawn = gameObject;
            normalESpawneds.Add(spawned);
        }
        
    }
}
