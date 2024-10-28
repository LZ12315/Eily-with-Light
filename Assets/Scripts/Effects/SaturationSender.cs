using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaturationSender : MonoBehaviour
{
    public static SaturationSender Instance;
    public Transform playerTransform;

    [Header("°ë¾¶¿ØÖÆ")]
    public float radius;
    public float minRadius = 2.0f;
    public float maxRadius = 2000f;
    public float maxDuration = 3f;
    private SaturationStatues statues = SaturationStatues.Work;
    public float radiusBreatheCoef = 0.65f;
    public float radiusBreatheSpeed = 0.5f;

    private float radiusStartValue;
    private float radiusEndValue;
    private float counter;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        Debug.Log(Instance.playerTransform);
        radiusStartValue = minRadius;
        radiusEndValue = minRadius * radiusBreatheCoef;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        switch(statues)
        {
            case SaturationStatues.Work:
                RadiusBreath();
                break;
            case SaturationStatues.TurnOn:
                TurnningOn(); 
                break;
            case SaturationStatues.TurnOff:
                TurnningOff();
                break;
        }
    }

    private void RadiusBreath()
    {
        float t = Mathf.PingPong(Time.time * radiusBreatheSpeed, 1f);
        radius = Mathf.Lerp(radiusStartValue, radiusEndValue, t);
    }

    private void TurnningOn()
    {
        if(counter < 1)
        {
            counter += Time.fixedDeltaTime;
            radius = Mathf.Lerp(radius, minRadius, counter/3);
        }
        else
        {
            counter = 0;
            statues = SaturationStatues.Work;
        }
    }

    private void TurnningOff()
    {
        if (counter < 1)
        {
            counter += Time.fixedDeltaTime;
            radius = Mathf.Lerp(radius, maxRadius, counter/maxDuration);
        }
        else
        {
            counter = 0;
            statues = SaturationStatues.Disable;
        }
    }

    public void ControlOn()
    {
        statues = SaturationStatues.TurnOn;
    }

    public void ControlOff()
    {
        statues = SaturationStatues.TurnOff;
    }

}
