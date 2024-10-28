using UnityEditor;
using UnityEngine;
using Cinemachine;
using static CustomInspectorObjects;

[CustomEditor(typeof(CameraControlTrigger))] // ×Ô¶¨Òåinspector
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
        if (cameraControlTrigger.customInspectorObjects.swapCameras)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnleft = EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObjects.cameraOnleft, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            cameraControlTrigger.customInspectorObjects.cameraOnright = EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObjects.cameraOnright, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }
        if (cameraControlTrigger.customInspectorObjects.panCameraContact)
        {
            cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", cameraControlTrigger.customInspectorObjects.panDirection);
            cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
            cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan time", cameraControlTrigger.customInspectorObjects.panTime);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}