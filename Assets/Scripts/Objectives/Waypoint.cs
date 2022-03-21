using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : Objective
{
    private void OnCollisionEnter(Collision collision)
    {
        //do waypoint things
        GameManagment.AddPoints(this.PointValue);
        this.Deactivate();
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }



}
