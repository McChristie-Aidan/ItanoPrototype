using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager _instance;
    public static ObjectiveManager Instance
    {
        get { return _instance;}
    }

    List<IObjective> objectives;
    List<IObjective> activeObjectives;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void AddObjective(Objective obj)
    {
        objectives.Add(obj);
    }

    void CheckInstance()
    {
        if (GameManagment.Instance == null)
        {
            GameManagment.CreateInstance();
        }
    }

    private void Start()
    {
        objectives = new List<IObjective>();
        activeObjectives = new List<IObjective>();
    }

    public static void CreateInstance()
    {
        if (ObjectiveManager.Instance == null)
        {
            GameObject GO = new GameObject("ObjectiveManager");
            GO.AddComponent<ObjectiveManager>();
            ObjectiveManager._instance = GO.GetComponent<ObjectiveManager>();
        }
    }
}
