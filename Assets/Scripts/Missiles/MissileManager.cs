using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    private static MissileManager _instance;
    public static MissileManager Instance { get { return _instance; } }

    public List<Missile> missiles;
    public List<Missile> activeMissiles;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        activeMissiles = new List<Missile>();
        missiles = new List<Missile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMissile(Missile missile)
    {
        this.activeMissiles.Add(missile);
    }

    public void RemoveMissile(Missile missile)
    {
        this.activeMissiles.Remove(missile);
    }

    public static void CreateInstance()
    {
        if (MissileManager.Instance == null)
        {
            GameObject GO = new GameObject("MissileManager");
            GO.AddComponent<MissileManager>();
            MissileManager._instance = GO.GetComponent<MissileManager>();
        }
    }

    public void DestroyAllMissiles()
    {
        for (int i = 0; i < activeMissiles.Count; i++)
        {
            activeMissiles[i].Die();
            activeMissiles.Remove(activeMissiles[i]);
        }  
    }
}
