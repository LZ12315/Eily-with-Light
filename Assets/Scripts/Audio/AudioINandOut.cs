using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioINandOut : MonoBehaviour
{
    public AudioClip audioClip; // ���������Ƶ�ļ�  
    public AudioSource audioSource; // �������AudioSource���  
    public float fadeInDuration = 2.0f; // �������ʱ��  
    public float fadeOutDuration = 2.0f; // ��������ʱ��  
    public bool isPlaying = false; // �����Ƶ�Ƿ��ڲ���  

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // ���û��AudioSource����������һ��  
        }
        audioSource.clip = audioClip; // ����AudioSource����Ƶ����  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �����봥����ʱ��ʼ���벥����Ƶ  
        if (!isPlaying)
        {
            StartCoroutine(FadeIn());
            isPlaying = true;
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    // ���뿪������ʱ��ʼ����ֹͣ��Ƶ  
    //    if (isPlaying)
    //    {
    //        StartCoroutine(FadeOut());
    //        isPlaying = false;
    //    }
    //}

    IEnumerator FadeIn()
    {
        audioSource.volume = 0.0f; // ����������Ϊ0��ʼ����  
        audioSource.Play(); // ������Ƶ  
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInDuration)
        {
            audioSource.volume = elapsedTime / fadeInDuration; // ����ʱ���������  
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡  
        }

        audioSource.volume = 0.25f; // ȷ�������ﵽ���ֵ  
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeOutDuration && audioSource.isPlaying)
        {
            audioSource.volume = 0.25f - (elapsedTime / fadeOutDuration); // ����ʱ���������  
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡  
        }

        audioSource.Stop(); // ֹͣ��Ƶ����  
    }
}
