using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

/// <summary>
/// 一个接口 定义数据存储相关的操作 所有需要被保存的对象都应该实现这个接口
/// 这些个函数按照顺序依次为：检查物体身上是否有数据存储必要组件的函数、将本物体注册到DataManager的函数、将本物体从DataManager注销的函数
/// 存储自己数据的函数、加载自己之前上传好的数据的函数
/// </summary>

public interface ISaveable
{
    public DataDefination GetDefination();

    public void RegisterSaveData() => DataManager.instance.savedObjects.Add(this);

    public void UnRegisterSaveData() => DataManager.instance.savedObjects.Remove(this);

    public void Save(SceneData data);

    public void Load(SceneData data);
}
