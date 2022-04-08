using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
     private TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GameObject.FindGameObjectWithTag("Points").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tmp != null)
        {
            tmp.text = $"Points: {GameManagment.Instance.pointTotal}";
        }
    }
}
