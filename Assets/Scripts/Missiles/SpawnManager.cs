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
    public int maxActiveSpawners;
    public float timeBetweenWaves = 30f, timeWaitForNextCountDown = 10f;
    public int missilesPerWave = 10;
    [HideInInspector]
    public int spawnersInScene, currentWaveNumber;
    [HideInInspector]
    public float waveTimeStamp;

    public bool useBiggerCooldown;
    bool firedThisWave = false;
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
        waveTimeStamp = Time.time + timeBetweenWaves;
    }

    private void Update()
    {
        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        if (Time.time > waveTimeStamp)
        {
            if (useBiggerCooldown)
            {               
                waveTimeStamp = Time.time + timeBetweenWaves;
                useBiggerCooldown = false;
            }
            else
            {              
                waveTimeStamp = Time.time + timeWaitForNextCountDown;
                useBiggerCooldown = true;
                firedThisWave = false;
            }
        }

        if (!useBiggerCooldown && Time.time > waveTimeStamp - timeWaitForNextCountDown/2 && !firedThisWave)
        {
            //fire wave
            foreach (Spawner spawner in spawners)
            {
                spawner.missilesPerWave += this.missilesPerWave;
            }
            firedThisWave = true;
            currentWaveNumber++;
        }
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
