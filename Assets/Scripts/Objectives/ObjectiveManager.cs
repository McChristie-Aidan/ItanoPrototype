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

    [SerializeField]
    int numberOfActiveObjectives = 7;
    int objectivesInScene = 0;

    List<Objective> objectives;
    List<Objective> activeObjectives;

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
    private void Start()
    {
        objectives = new List<Objective>();
        activeObjectives = new List<Objective>();
    }
    private void Update()
    {
        foreach (Objective item in objectives)
        {
            //remove inactive objectives
            if (!item.isActiveAndEnabled && activeObjectives.Contains(item))
            {
                activeObjectives.Remove(item);
            }

            if (activeObjectives.Count > numberOfActiveObjectives)
            {
                item.Deactivate();
            }
        }

        while (activeObjectives.Count < numberOfActiveObjectives)
        {
            //randomly engage a new objective
            int i = Random.Range(0, objectives.Count - 1);

            if (!objectives[i].isActiveAndEnabled)
            {
                objectives[i].Activate();

                activeObjectives.Add(objectives[i]);
            }
        }
    }

    public void AddObjective(Objective obj)
    {
        if (_instance == null)
        {
            CreateInstance();
        }
        objectives.Add(obj);
        objectivesInScene++;
    }

    void CheckInstance()
    {
        if (GameManagment.Instance == null)
        {
            GameManagment.CreateInstance();
        }
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
