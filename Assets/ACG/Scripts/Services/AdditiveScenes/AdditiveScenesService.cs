using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ACG.Scripts.Services
{
    public static class AdditiveScenesService 
    {
        #region Scenes Load

        public static async Task LoadAdditiveScenes(List<string> sceneNames)
        {
            if (sceneNames is null || sceneNames.Count == 0)
                return;

            foreach (string sceneName in sceneNames)
                await LoadAdditiveScene(sceneName);
        }
        public static async Task LoadAdditiveScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("Scene name is null or empty");
                return;
            }

            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (sceneLoadOperation.isDone is false)
                await Task.Yield();
        }

        #endregion

        #region Scenes Unload

        public static async Task UnloadAdditiveScenes(List<string> sceneNames)
        {
            if (sceneNames is null || sceneNames.Count == 0)
                return;

            foreach (string sceneName in sceneNames)
                await UnloadAdditiveScene(sceneName);
        }
        public static async Task UnloadAdditiveScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("Scene name is null or empty.");
                return;
            }

            if (SceneManager.GetSceneByName(sceneName).isLoaded is false)
            {
                Debug.LogWarning($"Scene {sceneName} is not currently loaded.");
                return;
            }

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            while (unloadOperation is { isDone: false })
                await Task.Yield();
        }

        #endregion
    }
}
