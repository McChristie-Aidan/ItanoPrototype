using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrantRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject[] Quadrants;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(Quadrants[Random.Range(0, Quadrants.Length)], this.transform, false);
        Debug.Log("Generated");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
