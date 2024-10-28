using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera virtualCamera;//虚拟相机
    public Collider2D boundary;//代表边界的碰撞器
    private void LateUpdate()
    {
        Vector3 cameraPos=virtualCamera.transform.position;

        if (boundary.bounds.Contains(new Vector2(cameraPos.x, cameraPos.y)))
        {
           
            Vector3 clampedPosition = cameraPos;
            clampedPosition.x = Mathf.Clamp(cameraPos.x, boundary.bounds.min.x, boundary.bounds.max.x);
            clampedPosition.y = Mathf.Clamp(cameraPos.y, boundary.bounds.min.y, boundary.bounds.max.y);
            clampedPosition.z = cameraPos.z;

            virtualCamera.transform.position = clampedPosition;
        }
    }
}
