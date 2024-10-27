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

    void OnTriggerEnter2D(Collider2D trigger)
    {
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            if (trigger.gameObject == triggerObjs[i])
            {
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

}
