using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera[] _allVirtualCameras;
    //public Camera[] _allMainCamera;
    [Header("���������/����ʱ�Ĳ�ֵ")]
    [SerializeField] private float _fallPanAmount = 0.5f;
    [SerializeField] private float _fallYPanTime = 0.25f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCoroutine;

    public CinemachineVirtualCamera _currentVitualCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    private Vector2 _startingTrakedObjectOffset;

    [Header("�����")]
    public CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        for (int i = 0; i < _allVirtualCameras.Length; i++)
        {
            if (_allVirtualCameras[i].enabled)
            {
                //Set currentCamera
                _currentVitualCamera = _allVirtualCameras[i];

                //set the framing transposer
                _framingTransposer = _currentVitualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        //Set the YDamping amount so it's based on the inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        //set the starting position of the tracked object offset
        _startingTrakedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //grab the starting damping amount
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampingAmount = 0f;

        //determine the end damping Amount
        if (isPlayerFalling)
        {
            endDampingAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampingAmount = _normYPanAmount;
        }

        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampingAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    #endregion

    #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        //handle pan from Trigger
        if (!panToStartingPos)
        {
            Debug.Log("������ص���ʼλ��");
            //set the direction and distance
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
            }

            endPos *= panDistance;

            startingPos = _startingTrakedObjectOffset;

            endPos += startingPos;
        }

        //handle the pan back to starting position
        else
        {
            Debug.Log("����ص���ʼλ��");
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrakedObjectOffset;
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));

            _framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }

    #endregion

    #region Swap Camera

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        //if the current camera is the camera on the Left and our trigger exit direction was on the right
        if (_currentVitualCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            Debug.Log("�������ҽ������");
            //active the new camera
            cameraFromRight.enabled = true;

            //deactive the old camera
            cameraFromLeft.enabled = false;

            //set the new camera as the current camera
            _currentVitualCamera = cameraFromRight;

            //update our composer variable
            _framingTransposer = _currentVitualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        if (_currentVitualCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            Debug.Log("�������󽻻����");
            //active the new camera
            cameraFromLeft.enabled = true;

            //deactive the old camera
            cameraFromRight.enabled = false;

            //set the new camera as the current camera
            _currentVitualCamera = cameraFromLeft;

            //update our composer variable
            _framingTransposer = _currentVitualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #endregion
}
