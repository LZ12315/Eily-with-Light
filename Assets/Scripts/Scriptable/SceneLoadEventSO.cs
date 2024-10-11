using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个SO文件 不负责存储数据 而是装载了一个Action 所有装载了这个SO文件的物体都可以注册这个Action 从而可以启动Action来广播消息 也可以接收Action广播的消息
/// sceneLoadEvent 是一个Action 负责场景切换 所有装载了这个SO文件的脚本都可以使用一个函数注册到Action来接收广播消息
/// RaiseSceneLoadEvent函数 负责启动sceneLoadEvent来广播消息
/// </summary>

[CreateAssetMenu(menuName = "EventSO/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public Action<GameSceneSO,bool,float> sceneLoadEvent;

    public void RaiseSceneLoadEvent(GameSceneSO sceneToLoad, bool fadeScreen, float fadeTime)
    {
        sceneLoadEvent?.Invoke(sceneToLoad,fadeScreen,fadeTime);
    }
}
