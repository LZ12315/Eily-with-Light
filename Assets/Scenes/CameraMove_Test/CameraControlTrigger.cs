using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnleft != null && customInspectorObjects.cameraOnright != null)
            {
                CameraManager.instance.SwapCamera(customInspectorObjects.cameraOnleft, customInspectorObjects.cameraOnright, exitDirection);
            }
            if (customInspectorObjects.panCameraContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false; // 镜头交换
    public bool panCameraContact = false; // 镜头平移衔接

    [HideInInspector] public CinemachineVirtualCamera cameraOnleft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnright;

    [HideInInspector] public PanDirection panDirection; // 方向
    [HideInInspector] public float panDistance = 3.0f; // 距离
    [HideInInspector] public float panTime = 0.35f; // 时间
}

public enum PanDirection // 方向枚举
{
    Up,
    Down,
    Left,
    Right
}
