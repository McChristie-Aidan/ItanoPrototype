using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraEffects : UnityEngine.MonoBehaviour
{
    public static CameraEffects Instance { get; private set; }

    CinemachineVirtualCamera vcam;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntesity;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.unscaledDeltaTime;
            CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (shakeTimer <= 0)
            {
                perlin.m_AmplitudeGain = 0f;
            }

            //perlin.m_AmplitudeGain = Mathf.Lerp(startingIntesity, 0f, (1 -(shakeTimer / shakeTimerTotal)));
        }
        else if (vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain > 0)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        }
    }
    public void ShakeCam(float intensity)
    {
        ShakeCam(intensity, .1f);
    }
    public void ShakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        startingIntesity = intensity;

        shakeTimer = time;
        shakeTimerTotal = time;
    }
    public void SetCamFOV(float targetFOV)
    {
        vcam.m_Lens.FieldOfView = targetFOV;
        //Debug.Log("set main cam fov to: " + targetFOV);
    }
}