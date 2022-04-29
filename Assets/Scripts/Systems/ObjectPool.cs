using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject prefab;
    public int maxSize;

    public Pool(GameObject Prefab, int PoolSize)
    {
        this.prefab = Prefab;
        this.maxSize = PoolSize;
    }
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    List<Pool> pools;

    Dictionary<string, Queue<GameObject>> poolDictionary;

    private static ObjectPool _instance;
    public static ObjectPool Instance { get { return _instance; } }

    private void Awake()
    {
        /*
         * deletes this game object of one instance of time manager exists already -A
         */
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    void Start()
    {
        foreach (Pool p in pools)
        {
            InstantiatePool(p);
        }
    }

    public GameObject GetObject(string objectName)
    {
        GameObject objectSpawn = poolDictionary[objectName].Dequeue();
        objectSpawn.SetActive(true);
        poolDictionary[objectName].Enqueue(objectSpawn);
        return objectSpawn;
    }

    public void DeactivateObject(GameObject ObjectType)
    {
        ObjectType.SetActive(false);
        Rigidbody rb = ObjectType.GetComponent<Rigidbody>();
    }

    public void InstantiatePool(Pool pool)
    {
        CreateObjectPoolInstance();
        GameObject temp;
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < pool.maxSize; i++)
        {
            //make object
            temp = Instantiate(pool.prefab);
            //disable it
            temp.SetActive(false);
            //add it to the pool
            objectPool.Enqueue(temp);
        }
        //add pool to the dictionary
        poolDictionary.Add(pool.prefab.name, objectPool);
        //pools.Add(pool);
    }

    public static void CreateObjectPoolInstance()
    {
        if (Instance == null)
        {
            GameObject opGameObject = new GameObject("ObjectPool");
            opGameObject.AddComponent<ObjectPool>();
            _instance = opGameObject.GetComponent<ObjectPool>();
        }
    }

    public bool CheckIfDictionaryHasValue(string name)
    {
        return poolDictionary.ContainsKey(name);
    }

    public bool CheckIfDictionaryHasValue(GameObject go)
    {
        return poolDictionary.ContainsKey(go.name);
    }
    public bool CheckIfDictionaryHasValue(Pool pool)
    {
        return poolDictionary.ContainsKey(pool.prefab.name);
    }
}