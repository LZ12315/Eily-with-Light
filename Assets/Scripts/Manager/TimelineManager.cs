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
    public GameObject BlackBoid;
    private CharCtrl CharCtrl;
    public BoidParent BoidParent;
    public GameObject pressKNote;

    void Awake()
    {
        transform.position = new Vector3(75.53f, -12.2f, 0);
        BlackBoid.SetActive(false);
        pressKNote.SetActive(false);
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            triggerObjs[i].gameObject.SetActive(true);
        }
    }
    void OnTriggerEnter2D(Collider2D trigger)
    {
        for (int i = 0; i < triggerObjs.Length; i++)
        {
            if (trigger.gameObject == triggerObjs[i])
            {
                curDirector = i + 1;
                PlayTimeline(i+1);
                trigger.gameObject.SetActive(false);
            }
        }
    }

    public void PlayTimeline(int directorNum)
    {
        timelineDirectors[directorNum].Play();
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
        BlackBoid.SetActive(true);
        isPlaying = true;
        pressKNote.SetActive(true);
    }

    public void SmallBall() 
    {
        for (int i = 0; i < BoidParent.boids.Count; i++)
        {
            BoidParent.boids[i].transform.GetChild(0).localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        
    }

}
