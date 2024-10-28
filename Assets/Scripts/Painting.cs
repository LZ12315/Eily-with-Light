using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    public SpriteRenderer spRenderer;
    public bool isShowing;
    public float curAlpha;
    // Start is called before the first frame update
    void Start()
    {
        isShowing = false;
        spRenderer = GetComponent<SpriteRenderer>();
        spRenderer.enabled = false;
    }
    void FixedUpdate()
    {
        if (isShowing&& spRenderer.color.a < 255f)
        {
                curAlpha += 1f * Time.deltaTime;
                spRenderer.color = new Color(255, 255, 255, curAlpha);
        }
    }
    
    public void ShowUp()
    {
        isShowing=true;
        spRenderer.enabled = true;
        spRenderer.color = new Color(255,255,255,0);
    }

    public void ToLight()
    {

    }
    public void ToDark()
    {

    }
}
