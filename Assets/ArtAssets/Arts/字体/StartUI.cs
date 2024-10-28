using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class StartUI : MonoBehaviour
{
     void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("按键按下！");
            if (!Input.GetKeyDown(KeyCode.Escape)&&!Input.GetMouseButtonDown(0)&!Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("GameScene");//任意键进入游戏
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("退出游戏");
                Application.Quit();//按下Esc键时，退出游戏
            }
        }
    }
}
