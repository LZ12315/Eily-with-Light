using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    public float fadeDuration = 3.0f;//�������ʱ��
    public bool fadeIn = true;//������ʾ����

    private Color initialColor;
    private Color targetColor;
    public bool isFading = false;

    void Start()
    {
        if(textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        initialColor = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0.0f);
        targetColor = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 1.0f);
       if( fadeIn )
        {
            StartCoroutine(FadeCoroutine(initialColor, targetColor));//��ʼЯ��
        }
        
    }
    // ���������������ʼ����  
    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(initialColor, targetColor));
        fadeIn = true;
    }

    // ���������������ʼ����  
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(targetColor, initialColor));
        fadeIn = false;
    }
    private IEnumerator FadeCoroutine(Color initialColor, Color targetColor)
    {
        isFading = true;
        float elapsedTime = 0.0f;

        while(elapsedTime < fadeDuration && isFading)
        {
           // Debug.Log("����");
            float t = elapsedTime / fadeDuration;
            textMeshPro.color = Color.Lerp(initialColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFading = false;
    }
   
}
    
