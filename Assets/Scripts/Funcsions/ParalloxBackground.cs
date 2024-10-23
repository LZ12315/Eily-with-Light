using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalloxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Vector3 cameraLatestPos;

    private void Start()
    {
        cameraLatestPos = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 dealtaMovement = cameraTransform.position - cameraLatestPos;
        //Vector2.Lerp()
    }
}
