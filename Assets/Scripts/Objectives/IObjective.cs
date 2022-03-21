using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{
    public int PointValue { get; }
    public abstract void Activate();
    public abstract void Deactivate();
}
