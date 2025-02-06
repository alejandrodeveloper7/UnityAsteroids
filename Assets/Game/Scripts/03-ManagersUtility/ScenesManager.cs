using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager
{
    public static ScenesManager Instance { get; } = new ScenesManager();

    #region Scenes Management

    public async void LoadScenesAdditive(List<string> pSceneNames, Action pOnComplete)
    {
        if (pSceneNames == null || pSceneNames.Count == 0)
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


    public async void UnloadScenes(List<string> sceneNames, Action onComplete)
    {
        if (sceneNames == null || sceneNames.Count == 0)
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
