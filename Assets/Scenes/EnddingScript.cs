using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnddingScript : MonoBehaviour
{
    public Image blackoutImage; // 引用画面变黑的UI Image  
    public Text continueText;   // 引用 Text  
    public float fadeInDuration = 1.0f;
   

    private void Start()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // 重置Image颜色为透明  
        continueText.gameObject.SetActive(false); // 隐藏Text  
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        continueText.text = "to be continue";
        continueText.color = Color.white; // 确保Text颜色为白色 
        StartCoroutine(EndingFadeIn());

       
    }

     IEnumerator  EndingFadeIn()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // 重置Image颜色为透明  
        blackoutImage.gameObject.SetActive(true);
        continueText.gameObject.SetActive(true);
   
        float elapsedTime = 0.0f;

        while (elapsedTime <= fadeInDuration)
        {
            
            blackoutImage.color = new Color(0, 0, 0, elapsedTime / fadeInDuration); 
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧  
        }
        blackoutImage.color = new Color(0, 0, 0, 1); 
    }
}
