using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Marker : MonoBehaviour
{
    public RectTransform prefab;
    private RectTransform marker;
    private TextMeshProUGUI tmp;

    private Transform player;

    Canvas canvas;

    Image img;

    float minX, maxX, minY, maxY;

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        marker = Instantiate(prefab, canvas.transform);
        tmp = marker.GetComponentInChildren<TextMeshProUGUI>();

        img = marker.GetComponent<Image>();
    }
    private void OnEnable()
    {
        marker.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        marker.gameObject.SetActive(false);
    }
    void Update()
    {
        minX = img.GetPixelAdjustedRect().width / 2;
        maxX = Screen.width - minX;

        minY = img.GetPixelAdjustedRect().height / 2;
        maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(this.transform.position);

        if (Vector3.Dot((transform.position - player.position), player.forward) < 0)
        {
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        marker.position = pos;

        tmp.text = Vector3.Distance(player.position, transform.position).ToString("0") + " m";
    }
}
