using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AdditiveScenesManager
{
    #region Scenes Management

    public static async void LoadAdditiveScenes(List<string> pSceneNames, Action pOnComplete)
    {
        if (pSceneNames == null || pSceneNames.Count == 0)
            return;

        foreach (string sceneName in pSceneNames)
            await LoadAdditiveScene(sceneName);

        pOnComplete?.Invoke();
    }
    private static async Task LoadAdditiveScene(string pSceneName)
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


    public static async void UnloadAdditiveScenes(List<string> pSceneNames, Action pOnComplete)
    {
        if (pSceneNames == null || pSceneNames.Count == 0)
            return;

        foreach (string sceneName in pSceneNames)
            await UnloadAdditiveScene(sceneName);

        pOnComplete?.Invoke();
    }
    private static async Task UnloadAdditiveScene(string pSceneName)
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
