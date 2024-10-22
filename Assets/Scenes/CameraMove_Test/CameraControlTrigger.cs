using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
        Debug.Log("���봥����");
        if (collision.CompareTag("Player"))
        {
            
            if(customInspectorObjects.panCameraContact)
            {
                //pan the camera;
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("�뿪������");
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnleft != null && customInspectorObjects.cameraOnright != null)
            {
                CameraManager.instance.SwapCamera(customInspectorObjects.cameraOnleft, customInspectorObjects.cameraOnright, exitDirection);
            }
            if (customInspectorObjects.panCameraContact)
            {
                //pan the camera;
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;//��ͷ����
    public bool panCameraContact=false;//��ͷƽ���ν�

    [HideInInspector] public CinemachineVirtualCamera cameraOnleft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnright;

    [HideInInspector] public PanDirection panDirection;//����
    [HideInInspector] public float panDistance = 3.0f;//����
    [HideInInspector] public float panTime = 0.35f;//ʱ��
}

public enum PanDirection//����ö��
{
    Up,
    Down, 
    Left,
    Right
}

[CustomEditor(typeof(CameraControlTrigger))]//�Զ���inspector
public class MyscriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(cameraControlTrigger.customInspectorObjects.swapCameras)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnleft = EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObjects.cameraOnleft, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            cameraControlTrigger.customInspectorObjects.cameraOnright = EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObjects.cameraOnright, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if(cameraControlTrigger.customInspectorObjects.panCameraContact)
        {
            cameraControlTrigger.customInspectorObjects.panDirection=(PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",cameraControlTrigger.customInspectorObjects.panDirection);
            cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance",cameraControlTrigger.customInspectorObjects.panDistance);
            cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan time",cameraControlTrigger.customInspectorObjects.panTime);
        }
        if(GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}

  