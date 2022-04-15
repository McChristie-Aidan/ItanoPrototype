using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get { return _instance; }
    }

    List<Spawner> spawners;
    int spawnersInScene;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            this.spawners = new List<Spawner>();
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        
    }

    public void AddSpawner(Spawner obj)
    {
        if (_instance == null)
        {
            CreateInstance();
        }
        spawners.Add(obj);
        spawnersInScene++;
    }

    public static void CreateInstance()
    {
        if (SpawnManager.Instance == null)
        {
            GameObject GO = new GameObject("SpawnManager");
            GO.AddComponent<SpawnManager>();
            SpawnManager._instance = GO.GetComponent<SpawnManager>();
        }
    }
}
