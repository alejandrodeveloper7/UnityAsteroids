using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    private GameSettings _gameSettings;


    #endregion

    #region Monobehaviour
    
    private void Start()
    {
        InitializeGame();
    }

    #endregion

    #region Initialization

    private void InitializeGame()
    {
        _gameSettings = ResourcesManager.Instance.GetScriptableObject<GameSettings>(ScriptableObjectKeys.GAME_SETTINGS_KEY);
        AdditiveScenesManager.LoadAdditiveScenes(_gameSettings._desktopSceneDependencys, OnAdditiveScenesLoadComplete);
    }

    private void OnAdditiveScenesLoadComplete()
    {
        EventManager.UIBus.RaiseEvent(new StartGame());
    }

    #endregion

}

public readonly struct StartGame : IEvent
{
}
