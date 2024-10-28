using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveControl : MonoBehaviour
{
    public float shockWaveTime = 0.5f;

    private Coroutine shockWaveCorotine;

    [SerializeField]private Material material;

    [SerializeField]private static int WaveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void CallShockWave(float startPos, float endPos)
    {
        shockWaveCorotine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
    }

    private IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        material.SetFloat(WaveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime <= shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / shockWaveTime);

            material.SetFloat(WaveDistanceFromCenter, lerpedAmount);
            yield return null;
        }

        material.SetFloat(WaveDistanceFromCenter, -0.1f);
    }
}
