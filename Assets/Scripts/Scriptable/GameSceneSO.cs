using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 一个SO文件 负责存储游戏场景本身 用于场景切换
/// sceneReference变量是一个Enum值 负责标记自己属于哪种类型的场景 后面的sceneReference变量负责存储场景本身 或者说场景在Addressable内的存储 
/// </summary>

[CreateAssetMenu(menuName = ("SceneSO/GameSceneSO"))]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
}
