using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    public AudioSource bgmAudio;
    public Slider volumeSlider;
    public List<AudioSource> audioSources;

    // Update is called once per frame
    void Update()
    {
       // bgmAudio.volume = volumeSlider.value;
        foreach (AudioSource audioSource in audioSources) { 
        
            audioSource.volume = volumeSlider.value;

        }
    }
}
