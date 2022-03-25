using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject objectToSpawn, spawnPoint, target;

    PlayerFlightControls pf;

    [SerializeField]
    float launchAngleVariation = 40f, spawnPointVariation = .5f;
 
    [SerializeField]
    float timeBetweenSpawns = 2f;
    float spawnTimeStamp;

    bool isOnCooldown;
    bool isFiring = true;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null)
            {
                pf = target.GetComponent<PlayerFlightControls>();
            }
        }

        MissileManager.CreateInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 15)
        {
            if (isFiring)
            {
                if (isOnCooldown)
                {
                    if (Time.time > spawnTimeStamp)
                    {
                        isOnCooldown = false;
                    }
                }
                else
                {
                    Spawn();
                    spawnTimeStamp = Time.time + timeBetweenSpawns;
                    isOnCooldown = true;
                }

                if (pf != null)
                {
                    if (!pf.isAlive)
                    {
                        isFiring = false;
                    }
                }
            }

        }
    }

    void Spawn()
    {
        GameObject go = Instantiate(
            objectToSpawn, 
            spawnPoint.transform.position + new Vector3(
                Random.Range(-spawnPointVariation, spawnPointVariation),
                0,
                Random.Range(-spawnPointVariation, spawnPointVariation)), 
            Quaternion.Euler(new Vector3(
                -90 + Random.Range(-launchAngleVariation, launchAngleVariation),
                Random.Range(-launchAngleVariation, launchAngleVariation),
                Random.Range(-launchAngleVariation, launchAngleVariation))), 
            this.transform);
        Missile missile = go.GetComponent<Missile>();

        if (missile != null)
        {
            missile.target = this.target;
        }
    }
}
