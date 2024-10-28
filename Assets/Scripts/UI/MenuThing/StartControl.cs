using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class StartControl : MonoBehaviour
{
    public GameObject textControl1;
    public GameObject textControl2;
    public GameObject text1;
    public GameObject text2;
    public TimelineManager timelineManager;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(textControl1);
                Destroy(textControl2);
                Destroy(text1);
                Destroy(text2);

                if (timelineManager != null)
                    timelineManager.PlayTimeline(0);
                Destroy(gameObject);
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("退出游戏");
                Application.Quit();//按下Esc键时，退出游戏
            }
        }
    }
}
