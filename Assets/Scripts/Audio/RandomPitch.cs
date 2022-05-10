using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    public float minPitch = .5f, maxPitch = 1.5f;

    AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
        a.pitch = Random.Range(minPitch, maxPitch);
    }
}
