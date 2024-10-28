using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    //������һ֡���λ��
    private Vector3 lastCameraPosition;

  
    void Start()
    {
        //��ȡ�����λ��
        cameraTransform = Camera.main.transform;
        //��ʼ����һ֡���λ��
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //��������ƶ�
        Vector3 deltaMovement=cameraTransform.position-lastCameraPosition;
        //�Ӳ�ƫ����
        float parallaxEffectMutiplier = .5f;
        //����λ��
        transform.position+=deltaMovement*parallaxEffectMutiplier;
        lastCameraPosition=cameraTransform.position+deltaMovement;
    }
}
