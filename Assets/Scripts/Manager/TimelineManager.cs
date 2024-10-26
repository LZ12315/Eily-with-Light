using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public GameObject[] triggerObjs;
    public PlayableDirector[] timelineDirectors;

    private void Start()
    {
        for (int i = 0; i < timelineDirectors.Length; i++)
        {
            if (i==0)
            {
                timelineDirectors[i].Play();
            }
            else
            {
                timelineDirectors[i].Stop();
            }
        }
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            if (collision.gameObject != triggerObjs[i])
            {
                timelineDirectors[i + 1].Stop();
            }
                if (collision.gameObject == triggerObjs[i])
            {
                timelineDirectors[i + 1].Play();
            }
        }
    }

}
