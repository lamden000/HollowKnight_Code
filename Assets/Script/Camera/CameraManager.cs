using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y damping during player jump/fall")]
    private Coroutine _lerdYPanCoroutine;
    [SerializeField] private float _fallPanAmount=0.25f;
    [SerializeField] private float _fallPanYTime=0.35f;
    public float _fallSpeedYDampingChangeThreshold=-15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutime;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    void Awake()
    {
        if (instance == null)
            instance = this;
        for(int i=0;i<_allVirtualCameras.Length;i++)
        {
            if( _allVirtualCameras[i].enabled)
            {
                _currentCamera = _allVirtualCameras[i];
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        _normYPanAmount = _framingTransposer.m_YDamping;
    }

    public void LerpYDamoino(bool isPlayerfalling)
    {
        _lerpYPanCoroutime = StartCoroutine(LerpYAction(isPlayerfalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0;

        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount=_normYPanAmount;
        }

        float elapsedTime = 0;
        while(elapsedTime<_fallPanYTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallPanYTime));

            yield return null;
        }

        IsLerpingYDamping = false;
    }
}
