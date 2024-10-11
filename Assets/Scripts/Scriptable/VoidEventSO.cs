using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个SO文件 不负责存储数据 而是装载了一个Action 所有装载了这个SO文件的物体都可以注册这个Action 从而可以启动Action来广播消息 也可以接收Action广播的消息
/// voidEvent 是一个Action 但不传递任何参数 也就是说单纯是为了广播消息 所有装载了这个SO文件的脚本都可以使用一个函数注册到Action来接收广播消息
/// RaiseVoidEvent函数 负责启动voidEvent来广播消息
/// </summary>

[CreateAssetMenu(menuName = "EventSO/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    public Action voidEvent;

    public void RaiseVoidEvent()
    {
        voidEvent?.Invoke();
    }
}
