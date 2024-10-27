using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cameraTransform;
    private Vector3 lastCameraPosition;

  
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition=cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement=cameraTransform.position-lastCameraPosition;
        float parallaxEffectMutiplier = .5f;
        transform.position+=deltaMovement*parallaxEffectMutiplier;
        lastCameraPosition=cameraTransform.position+deltaMovement;
    }
}
