using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnddingScript : MonoBehaviour
{
    public Image blackoutImage; // ���û����ڵ�UI Image  
    public Text continueText;   // ���� Text  
    public float fadeInDuration = 1.0f;
   

    private void Start()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // ����Image��ɫΪ͸��  
        continueText.gameObject.SetActive(false); // ����Text  
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        continueText.text = "to be continue";
        continueText.color = Color.white; // ȷ��Text��ɫΪ��ɫ 
        StartCoroutine(EndingFadeIn());

       
    }

     IEnumerator  EndingFadeIn()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // ����Image��ɫΪ͸��  
        blackoutImage.gameObject.SetActive(true);
        continueText.gameObject.SetActive(true);
   
        float elapsedTime = 0.0f;

        while (elapsedTime <= fadeInDuration)
        {
            
            blackoutImage.color = new Color(0, 0, 0, elapsedTime / fadeInDuration); 
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡  
        }
        blackoutImage.color = new Color(0, 0, 0, 1); 
    }
}
