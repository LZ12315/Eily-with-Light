using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Checker : MonoBehaviour
{
    //如果玩家掉出屏幕，Destroy玩家，返回存档点
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
    
}
