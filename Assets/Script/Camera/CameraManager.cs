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

    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    private Vector2 _startingTrackedObjectOffset;
    public CinemachineVirtualCamera GetCurrentCamera()
    {
        return _currentCamera;
    }

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

        _startingTrackedObjectOffset =_framingTransposer.m_TrackedObjectOffset;
      
    }

    private void Start()
    {
        CameraShake.instance.UpdateCurrentCamera(_currentCamera);
    }

    public void LerpYDamping(bool isPlayerfalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerfalling));
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

    #region Pan Camera
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection,bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance,panTime,panDirection,panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos= Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                default:
                    break;
            }

            endPos *= panDistance;

            startingPos = _startingTrackedObjectOffset;

            endPos += startingPos;
        }
        else
        {
            startingPos= _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime<panTime)
        {
            elapsedTime+= Time.deltaTime;
            Vector3 panLerp=Vector3.Lerp(startingPos, endPos, elapsedTime/panTime);
            _framingTransposer.m_TrackedObjectOffset= panLerp;

            yield return null;
        }
    }
    #endregion
    #region Swap Cameras
    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 trigerExistDirection)
    {
        if (_currentCamera == cameraFromLeft && trigerExistDirection.x > 0)
        {
            cameraFromRight.enabled=true;
            cameraFromLeft.enabled=false;
            _currentCamera = cameraFromRight;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            CameraShake.instance?.UpdateCurrentCamera(_currentCamera);
        }
        else if (_currentCamera == cameraFromRight && trigerExistDirection.x < 0)
        {
            cameraFromRight.enabled = false;
            cameraFromLeft.enabled = true;
            _currentCamera = cameraFromLeft;
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            CameraShake.instance?.UpdateCurrentCamera(_currentCamera);
        }
    }
    #endregion

}
