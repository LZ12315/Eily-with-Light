using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidHitVFX : MonoBehaviour
{
    public float existTime = 0.5f;

    private void OnEnable()
    {
        Destroy(gameObject,existTime);
    }
}
