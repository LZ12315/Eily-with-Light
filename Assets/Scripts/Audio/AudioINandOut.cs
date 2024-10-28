using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioINandOut : MonoBehaviour
{
    public AudioClip audioClip; // 引用你的音频文件  
    public AudioSource audioSource; // 引用你的AudioSource组件  
    public float fadeInDuration = 2.0f; // 渐入持续时间  
    public float fadeOutDuration = 2.0f; // 渐出持续时间  
    public bool isPlaying = false; // 标记音频是否在播放  

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 如果没有AudioSource组件，则添加一个  
        }
        audioSource.clip = audioClip; // 设置AudioSource的音频剪辑  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 当进入触发器时开始渐入播放音频  
        if (!isPlaying)
        {
            StartCoroutine(FadeIn());
            isPlaying = true;
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    // 当离开触发器时开始渐出停止音频  
    //    if (isPlaying)
    //    {
    //        StartCoroutine(FadeOut());
    //        isPlaying = false;
    //    }
    //}

    IEnumerator FadeIn()
    {
        audioSource.volume = 0.0f; // 将音量设置为0开始渐入  
        audioSource.Play(); // 播放音频  
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInDuration)
        {
            audioSource.volume = elapsedTime / fadeInDuration; // 根据时间更新音量  
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧  
        }

        audioSource.volume = 0.25f; // 确保音量达到最大值  
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeOutDuration && audioSource.isPlaying)
        {
            audioSource.volume = 0.25f - (elapsedTime / fadeOutDuration); // 根据时间更新音量  
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧  
        }

        audioSource.Stop(); // 停止音频播放  
    }
}
