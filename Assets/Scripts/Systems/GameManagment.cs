using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagment : MonoBehaviour
{
    private static GameManagment _instance;
    public static GameManagment Instance
    {
        get { return _instance; }
    }

    [HideInInspector]
    public int pointTotal;

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

    public static void AddPoints(int points)
    {
        if (GameManagment.Instance == null)
        {
            GameManagment.CreateInstance();
        }

        GameManagment.Instance.pointTotal += points;
    }

    public static void CreateInstance()
    {
        if (GameManagment.Instance == null)
        {
            GameObject GO = new GameObject("GameManagment");
            GO.AddComponent<GameManagment>();
            GameManagment._instance = GO.GetComponent<GameManagment>();
        }
    }
}
