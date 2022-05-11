using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UIParticleHandler : MonoBehaviour
{
    private static UIParticleHandler _instance;
    public static UIParticleHandler Instance
    {
        get { return _instance; }
    }

    private Dictionary<string, VisualEffect> effects;
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
        VisualEffect[] visualEffects = GetComponentsInChildren<VisualEffect>();
        effects = new Dictionary<string, VisualEffect>();
        foreach (VisualEffect vis in visualEffects)
        {
            effects.Add(vis.name, vis);
        }
    }
    public void PlayParticle(string targetEffect)
    {
        effects[targetEffect].Stop();
        effects[targetEffect].Play();
    }

}
