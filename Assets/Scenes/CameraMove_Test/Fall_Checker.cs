using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Checker : MonoBehaviour
{
    //�����ҵ�����Ļ��Destroy��ң����ش浵��
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
    
}
