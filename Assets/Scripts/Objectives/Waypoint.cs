using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : Objective
{
    public AudioSource ringSound;

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //play Sound
            if (ringSound != null)
            {
                ringSound.Play();
            }

            //do waypoint things
            GameManagment.AddPoints(this.PointValue);
            this.Deactivate(); 
        }
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
