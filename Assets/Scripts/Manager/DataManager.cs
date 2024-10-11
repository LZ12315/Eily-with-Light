using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个Manager脚本 负责数据保存相关操作
/// 主体是SaveSceneData和LoadSavedSceneData函数 前者负责在切换场景前存储旧场景内数据 后者负责加载新场景存储好的数据
/// 遵循观察者模式的思维 该脚本装载在场景“Persistent”的物体“DataManager”上
/// Persistent场景始终存在 里面保存着所有的Manager 负责游戏的内部逻辑实现
/// </summary>

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header("数据保存")]
    public List<ISaveable> savedObjects = new List<ISaveable>();
    public SceneData NowSceneData;


    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void SaveSceneData()
    {
        if (savedObjects.Count <= 0)
            return;

        foreach(var saveables in savedObjects)
        {
            if(saveables.GetDefination().persistentType == PersistentType.ReadWrite)
                saveables.Save(NowSceneData);
        }
    }

    public void LoadSavedSceneData()
    {
        if (savedObjects.Count <= 0)
            return;

        foreach (var saveables in savedObjects)
        {
            if (saveables.GetDefination().persistentType == PersistentType.ReadWrite)
                saveables.Load(NowSceneData);
        }
    }

    public void SaveArchiveData()
    {

    }

    public void LoadSavedArchiveData()
    {

    }
}
