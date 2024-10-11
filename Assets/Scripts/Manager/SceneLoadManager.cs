using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// 一个Manager脚本 负责场景转换相关逻辑 
/// 场景转换的主体是OnLoadScene函数 负责接收场景转换的消息 以及启动卸载旧场景、加载新场景、过场动画等功能的相关函数 
/// 遵循观察者模式的思维 该脚本装载在场景“Persistent”的物体“SceneLoadManager”上
/// Persistent场景始终存在 里面保存着所有的Manager 负责游戏的内部逻辑实现
/// </summary>

public class SceneLoadManager : MonoBehaviour
{
    [Header("场景")]
    public GameSceneSO firstLoadScene;

    [Header("信息接收")]
    public SceneLoadEventSO sceneLoadEvent;

    [Header("广播")]
    public VoidEventSO beforeUnLoadEvent;
    public VoidEventSO afterLoadEvent;

    [Header("场景切换")]
    private bool isLoading;
    private GameSceneSO currentScene;
    private GameSceneSO sceneToLoad;

    [Header("加载动画")]
    private bool fadeScreen;
    private float fadeTime;

    private void Awake()
    {
        if(firstLoadScene != null)
        {
            OnLoadScene(firstLoadScene, false, 0);
        }
    }

    private void OnEnable()
    {
        sceneLoadEvent.sceneLoadEvent += OnLoadScene;
    }

    private void OnDisable()
    {
        sceneLoadEvent.sceneLoadEvent -= OnLoadScene;
    }

    private void OnLoadScene(GameSceneSO sceneToLoad, bool fadeScreen, float fadeTime)
    {
        if(isLoading)
        {
            return;
        }
        isLoading = true;
        this.sceneToLoad = sceneToLoad;
        this.fadeScreen = fadeScreen;
        this.fadeTime = fadeTime;

        if(currentScene != null )
        {
            StartCoroutine(UnLoadCurrentScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadCurrentScene()
    {
        if(fadeScreen)
        {
            //fade in
        }
        DataManager.instance.SaveSceneData();
        yield return new WaitForSeconds(fadeTime);
        yield return currentScene.sceneReference.UnLoadScene();
        LoadNewScene() ;
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        DataManager.instance.LoadSavedSceneData();
        currentScene = sceneToLoad;
        isLoading = false;
        afterLoadEvent.RaiseVoidEvent();
        if (fadeScreen)
        {
            //Fadeout
        }
        
        Scene sceneToActive = handle.Result.Scene;
        if (sceneToActive.IsValid())
        {
            SceneManager.SetActiveScene(sceneToActive);
        }
    }
}
