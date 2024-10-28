using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnddingScript : MonoBehaviour
{
    public Image blackoutImage; // ���û����ڵ�UI Image  
    public Text continueText;   // ���� Text  
    public float fadeInDuration = 1.0f;
    public bool isTriggered = false;

    private void Start()
    {
        blackoutImage.color = new Color(0, 0, 0, 0); // ����Image��ɫΪ͸��  
        continueText.gameObject.SetActive(false); // ����Text  
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered)
        {
            blackoutImage.gameObject.SetActive(true);
            blackoutImage.color = new Color(0, 0, 0, 0); // ����Image��ɫΪ͸��  
            continueText.gameObject.SetActive(true) ;
            isTriggered = true;
            blackoutImage.color = new Color(0, 0, 0, 1); // ����Image��ɫΪ��ɫ��͸����Ϊ1  
            continueText.text = "to be continue";
            continueText.color = Color.white; // ȷ��Text��ɫΪ��ɫ  
            continueText.gameObject.SetActive(true); // ȷ��Text�Ǽ���״̬  
        }
    }

   
}
