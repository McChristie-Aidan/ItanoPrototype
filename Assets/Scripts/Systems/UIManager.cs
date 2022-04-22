using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI points;
    public TextMeshProUGUI waveCount, warning, waveTimer, missileCount;
    PlayerFlightControls player;
    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.FindGameObjectWithTag("Points").GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFlightControls>();
        warning.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //update points
        Points();
        //Update wave count
        WaveCount();
        //update warning sign
        Warning();
        //update wave timer
        WaveTimer();

        MissileCount();
    }
    void Points()
    {
        if (points != null)
        {
            points.text = $"Points: {GameManagment.Instance.pointTotal}";
        }
    }
    void WaveCount()
    {
        if (waveCount != null)
        {
            if (SpawnManager.Instance.currentWaveNumber > 0)
            {
                waveCount.text = $"Wave: {SpawnManager.Instance.currentWaveNumber}";
            }
            else
            {
                waveCount.text = "";
            }
        }
    }
    void Warning()
    {
        if (warning != null)
        {
            if (SpawnManager.Instance.useBiggerCooldown && SpawnManager.Instance.currentWaveNumber > 0)
            {
                if (warning.color.a < .1)
                {
                    warning.color = new Color(1, 0, 0, 1);
                }
                else
                {
                    //reduces any remaining alpha to 0
                    warning.color = new Color(
                        warning.color.r,
                        warning.color.g,
                        warning.color.b,
                        warning.color.a - Time.deltaTime);
                }
            }
            else       
            {
                if (warning.color.a > 0)
                {
                    //reduces any remaining alpha to 0
                    warning.color = new Color(
                        warning.color.r,
                        warning.color.g,
                        warning.color.b,
                        warning.color.a - Time.deltaTime);
                }
            }
        }
    }
    void WaveTimer()
    {
        if (waveTimer != null)
        {
            if (player.isAlive)
            {
                if (SpawnManager.Instance.useBiggerCooldown)
                {
                    waveTimer.text = $"Next Wave In: {(SpawnManager.Instance.waveTimeStamp - Time.time).ToString("0.00")}";
                }
                else
                {
                    waveTimer.text = $"Spawning Current Wave: {(SpawnManager.Instance.waveTimeStamp - Time.time).ToString("0.00")}";
                }
            }
        }
    }
    void MissileCount()
    {
        if (missileCount != null)
        {
            if (MissileManager.Instance.activeMissiles.Count > 0)
            {
                missileCount.text = $"Missiles left: {MissileManager.Instance.activeMissiles.Count.ToString()}";
            }
            else
            {
                missileCount.text = "";
            }
            
        }
    }
}
