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
            Debug.Log("�������£�");
            if (!Input.GetKeyDown(KeyCode.Escape)&&!Input.GetMouseButtonDown(0)&!Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("GameScene");//�����������Ϸ
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("�˳���Ϸ");
                Application.Quit();//����Esc��ʱ���˳���Ϸ
            }
        }
    }
}
