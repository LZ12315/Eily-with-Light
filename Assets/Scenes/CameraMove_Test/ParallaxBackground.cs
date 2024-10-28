using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    //保存上一帧相机位置
    private Vector3 lastCameraPosition;

  
    void Start()
    {
        //获取主相机位置
        cameraTransform = Camera.main.transform;
        //初始化上一帧相机位置
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //计算相机移动
        Vector3 deltaMovement=cameraTransform.position-lastCameraPosition;
        //视差偏移量
        float parallaxEffectMutiplier = .5f;
        //更新位置
        transform.position+=deltaMovement*parallaxEffectMutiplier;
        lastCameraPosition=cameraTransform.position+deltaMovement;
    }
}
