using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI points;
    public TextMeshProUGUI waveCount, warning, waveTimer, missileCount;
    public AudioSource warningSound;
    
    [SerializeField]
    private CanvasGroup pauseMenu;
    
    public static bool isPaused;

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
        //update pause menu
        PauseMenu();

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
                //play sound effect
                if (!warningSound.isPlaying)
                {
                    warningSound.Play();
                }

                //change color
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
                //turn off sound effect
                if (warningSound.isPlaying)
                {
                    warningSound.Stop();
                }

                //reset color to invisible
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

    void PauseMenu()
    {
        if (isPaused)
        {
            pauseMenu.interactable = true;
            pauseMenu.alpha = 1;
        }
        else if (!isPaused)
        {
            pauseMenu.interactable = false;
            pauseMenu.alpha = 0;
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
