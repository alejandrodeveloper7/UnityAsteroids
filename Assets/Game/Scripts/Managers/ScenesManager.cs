using System;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    #region Singleton

    private static ScenesManager m_instance;
    public static ScenesManager Instance => m_instance;

    private void GenerateSingleton()
    {
        if (m_instance != null)
            Destroy(this);
        else
            m_instance = this;
    }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GenerateSingleton();
    }

    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<LoadScenesAdditive>(OnLoadScenesAdditive);
        EventManager.GetUiBus().AddListener<UnLoadScenesAdditive>(OnUnloadScenesAdditive);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().RemoveListener<LoadScenesAdditive>(OnLoadScenesAdditive);
        EventManager.GetUiBus().RemoveListener<UnLoadScenesAdditive>(OnUnloadScenesAdditive);
    }

    #endregion

    #region Bus Callbacks

    private void OnLoadScenesAdditive(LoadScenesAdditive pLoadScenesAdditive)
    {
        LoadScenesAdditive(pLoadScenesAdditive.ScenesName, pLoadScenesAdditive.OnComplete);
    }

    private void OnUnloadScenesAdditive(UnLoadScenesAdditive pUnLoadScenesAdditive)
    {
        LoadScenesAdditive(pUnLoadScenesAdditive.ScenesName, pUnLoadScenesAdditive.OnComplete);
    }

    #endregion

    #region Scenes Management

    public async void LoadScenesAdditive(string[] pSceneNames, Action pOnComplete)
    {
        if (pSceneNames == null || pSceneNames.Length == 0)
            return;

        foreach (string sceneName in pSceneNames)
            await LoadSceneAdditive(sceneName);

        pOnComplete?.Invoke();
    }
    public async Task LoadSceneAdditive(string pSceneName)
    {
        if (string.IsNullOrEmpty(pSceneName))
        {
            Debug.LogWarning("Scene name is null or empty");
            return;
        }

        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(pSceneName, LoadSceneMode.Additive);
        while (sceneLoadOperation.isDone is false)
            await Task.Yield();
    }


    public async void UnloadScenes(string[] sceneNames, Action onComplete)
    {
        if (sceneNames == null || sceneNames.Length == 0)
            return;

        foreach (string sceneName in sceneNames)
            await UnloadScene(sceneName);

        onComplete?.Invoke();
    }
    public async Task UnloadScene(string pSceneName)
    {
        if (string.IsNullOrEmpty(pSceneName))
        {
            Debug.LogWarning("Scene name is null or empty.");
            return;
        }

        if (SceneManager.GetSceneByName(pSceneName).isLoaded is false)
        {
            Debug.LogWarning(string.Format("Scene {0} is not currently loaded.", pSceneName));
            return;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(pSceneName);
        while (unloadOperation is { isDone: false })
            await Task.Yield();
    }

    #endregion
}

#region IEvents

public class LoadScenesAdditive : IEvent
{
    public string[] ScenesName { get; set; }
    public Action OnComplete { get; set; }
}

public class UnLoadScenesAdditive : IEvent
{
    public string[] ScenesName { get; set; }
    public Action OnComplete { get; set; }
}

#endregion
