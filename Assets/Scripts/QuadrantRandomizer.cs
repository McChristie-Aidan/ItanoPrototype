using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuadrantRandomizer : MonoBehaviour
{
    private List<GameObject> QuadrantList;
    // Start is called before the first frame update
    void Start()
    {
        //instantiate the list
        QuadrantList = new List<GameObject>();

        //find all the quadrant prefabs in the 'Assets/Resources/Prefabs/Quadrants' folder and add them to the list
        foreach(var quadrant in Resources.LoadAll<GameObject>("Prefabs/Quadrants"))
        {
            QuadrantList.Add(quadrant); 
        }

        Debug.Log(QuadrantList.Count);

        //randomly generate the position of the quadrant
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(QuadrantList[Random.Range(0, QuadrantList.Count)], this.transform, false);
        Debug.Log("Generated");
    }
}
