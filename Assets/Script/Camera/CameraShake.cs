using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _perlinNoise;
    private float shakeTimer;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f && _perlinNoise != null)
            {
                _perlinNoise.m_AmplitudeGain = 0f;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="newCamera">CinemachineVirtualCamera m?i</param>
    public void UpdateCurrentCamera(CinemachineVirtualCamera newCamera)
    {
        cinemachineVirtualCamera = newCamera;

        if (cinemachineVirtualCamera != null)
        {
            _perlinNoise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        else
        {
            Debug.LogWarning("No CinemachineVirtualCamera provided to CameraShake.");
        }
    }

    /// <summary>
    /// Rung camera v?i c??ng ?? và th?i gian ch? ??nh
    /// </summary>
    /// <param name="intensity">C??ng ?? rung</param>
    /// <param name="time">Th?i gian rung</param>
    public void ShakeCamera(float intensity, float time)
    {
        if (_perlinNoise == null)
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component is missing or not initialized!");
            return;
        }

        _perlinNoise.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}
