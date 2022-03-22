using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IObjective
{
    protected virtual void Start()
    {
        ObjectiveManager.Instance.AddObjective(this);
        this.Deactivate();
    }

    [SerializeField]
    private int pointValue;
    [HideInInspector]
    public int PointValue
    {
        get { return pointValue; }
    }

    public virtual void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
