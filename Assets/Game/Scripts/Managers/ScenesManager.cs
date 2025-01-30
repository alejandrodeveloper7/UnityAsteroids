using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    
    public async void LoadScenesAdditive(string[] pSceneNames)
    {
        if (pSceneNames == null || pSceneNames.Length == 0)
            return;

        foreach (string sceneName in pSceneNames)
            await LoadSceneAdditive(sceneName);
    }

    public async Task LoadSceneAdditive(string pSceneName)
    {
        if (!string.IsNullOrEmpty(pSceneName))
        {
            Debug.LogWarning("Scene name null or empty");
            return;
        }

        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(pSceneName, LoadSceneMode.Additive);
        while (sceneLoadOperation.isDone is false)
            await Task.Yield();
    }

}
