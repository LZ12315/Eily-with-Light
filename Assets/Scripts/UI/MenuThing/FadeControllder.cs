using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControllder : MonoBehaviour
{
    public TextFade textFade;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        textFade.FadeIn();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        textFade.FadeOut();
    }
}
