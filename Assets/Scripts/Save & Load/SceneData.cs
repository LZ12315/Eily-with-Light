using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个SO文件 负责存储游戏场景内需要保存对象的数据
/// sceneDataNumber变量负责标记自己属于哪个存档的数据 后面两个字典负责存储数据
/// </summary>

[CreateAssetMenu (menuName = "DataSO/SceneData")]
public class SceneData : ScriptableObject
{
    public ArchieveNumber sceneDataNumber;

    public Dictionary<string, string> stringDictionary = new Dictionary<string, string>();
    public Dictionary<string, bool> boolDictionary = new Dictionary<string, bool>();
}
