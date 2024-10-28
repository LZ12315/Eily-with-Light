using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnddingScript : MonoBehaviour
{
    public Image blackoutImage; // 引用画面变黑的UI Image  
    public Text continueText;   // 引用 Text  
    public float fadeInDuration = 1.0f;
    public bool isTriggered = false;

    private void Start()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // 重置Image颜色为透明  
        continueText.gameObject.SetActive(false); // 隐藏Text  
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered)
        {
            blackoutImage.gameObject.SetActive(true);
            blackoutImage.color = new Color(0, 0, 0, 0); // 重置Image颜色为透明  
            continueText.gameObject.SetActive(true) ;
            isTriggered = true;
            blackoutImage.color = new Color(0, 0, 0, 1); // 设置Image颜色为黑色，透明度为1  
            continueText.text = "to be continue";
            continueText.color = Color.white; // 确保Text颜色为白色  
            continueText.gameObject.SetActive(true); // 确保Text是激活状态  
        }
    }

   
}
