using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour, IObjective
{
    [SerializeField]
    private int pointValue;
    [HideInInspector]
    public int PointValue
    {
        get { return pointValue; }
    }

    public virtual void Activate()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
