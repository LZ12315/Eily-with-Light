using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public GameObject[] triggerObjs;
    public PlayableDirector[] timelineDirectors;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            if (collision.gameObject == triggerObjs[i])
            {
                PlayTimeline(timelineDirectors[i+1]); 
            }
        }
    }

    void PlayTimeline(PlayableDirector director)
    {
        director.Play();
    }
}
