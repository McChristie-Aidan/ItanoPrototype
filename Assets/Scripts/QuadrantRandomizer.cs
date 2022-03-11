using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuadrantRandomizer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> QuadrantList;
    // Start is called before the first frame update
    void Start()
    {
        QuadrantList = new List<GameObject>();

        foreach(var quadrant in Resources.LoadAll<GameObject>("Prefabs/Quadrants"))
        {
            QuadrantList.Add(quadrant); 
        }

        Debug.Log(QuadrantList.Count);

        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(QuadrantList[Random.Range(0, QuadrantList.Count)], this.transform, false);
        Debug.Log("Generated");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
