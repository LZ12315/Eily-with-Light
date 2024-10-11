using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个接口 定义玩家交互相关的操作 所有场景物体、UI按键等都应该实现这个接口
/// 三个函数按照顺序依次为：负责检测交互事件的函数、检查是否可以交互的函数、启动交互行为的函数
/// </summary>

public interface IInteractable
{
    public void ReceiveInteraction();

    public bool CheckIfInteractable();

    public void ExcutiveAction();
}
