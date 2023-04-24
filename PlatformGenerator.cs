using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{   

    [Header("Platform Manager")]
    
    [SerializeField] List<GameObject> shortPlatformPrefabs;

    [SerializeField] List<GameObject> longPlatformPrefabs;

    [SerializeField] List<GameObject> shortPlatformSpawnLocations;

    [SerializeField] List<GameObject> longPlatformSpawnLocations;
    void Start()
    {
        SpawnPlatforms(shortPlatformPrefabs, shortPlatformSpawnLocations);
        SpawnPlatforms(longPlatformPrefabs, longPlatformSpawnLocations);
    }
    // after level 4 we use this fuction in order to have different levels.
    void SpawnPlatforms(List<GameObject> prefabslist, List<GameObject> transformslist)
    {  
        int prefabRand = UnityEngine.Random.Range(0,prefabslist.Count);
        for (int i = 0; i < transformslist.Count; i++)
        {
            if(transformslist[i] && prefabslist[i])
            {
                GameObject spawnedPlatform = Instantiate(prefabslist[prefabRand],
                transformslist[i].transform.position
                ,Quaternion.Euler(0,-90,0));

                spawnedPlatform.transform.parent = GameObject.Find("Platforms").transform;
                spawnedPlatform.transform.localScale = new Vector3(spawnedPlatform.transform.localScale.x, 
                spawnedPlatform.transform.localScale.y, 4.18f);
            }
        }
    }
}
