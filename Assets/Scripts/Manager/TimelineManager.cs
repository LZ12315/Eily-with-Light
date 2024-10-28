using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public GameObject[] triggerObjs;
    public PlayableDirector[] timelineDirectors;
    public bool isPlaying;
    public int curDirector=0;
    public CinemachineVirtualCamera follow1;
    public CinemachineVirtualCamera follow2;
    private CharCtrl CharCtrl;

    void Awake()
    {
        transform.position = new Vector3(75.53f, -12.2f, 0);
    }
    void OnTriggerEnter2D(Collider2D trigger)
    {
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            if (trigger.gameObject == triggerObjs[i])
            {
                curDirector = i + 1;
                PlayTimeline(timelineDirectors[i + 1]);
            }
        }
    }

    void PlayTimeline(PlayableDirector director)
    {
        director.Play();
    }

    public void TimelineStartSignal()
    {
        isPlaying = true;
    }
    public void TimelineStopSignal()
    {
        isPlaying= false;
    }

    public void SwitchToSmall()
    {
        transform.localScale *=0.3f;
        CharCtrl.particle.transform.localScale *= 0.3f;
        follow1.gameObject.SetActive(false);
        follow2.gameObject.SetActive(true);

    }
}
