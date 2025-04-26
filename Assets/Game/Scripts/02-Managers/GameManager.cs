using ToolsACG.Utils.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields

    private GameSettings _gameSettings;

    private bool _inMainMenu;
    private bool _inMatch;
    private bool _inPause;
    private bool _inLeaderboard;

    #endregion

    #region Monobehaviour
    
    private void Start()
    {
        InitializeGame();
    }

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
        EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus Callbacks

    private void OnStartMatch(StartMatch pStartMatch)
    {
        Debug.Log("- PROGRESS - Match Started");
        _inMainMenu = false;
        _inMatch = true;
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        Debug.Log("- PROGRESS - Match Finished");
        _inMatch = false;
        _inLeaderboard = true;
    }

    private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked) 
    {
        _inLeaderboard = false;
        _inMainMenu = true;
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        Debug.Log("- PROGRESS - Game leaved");
        _inMatch = false;
        _inPause = false;
        _inMainMenu = true;
    }

    #endregion

    #region Initialization

    private void InitializeGame()
    {
        _gameSettings = ResourcesManager.Instance.GetScriptableObject<GameSettings>(ScriptableObjectKeys.GAME_SETTINGS_KEY);

        ScreenManager.SetTargetFrameRate(_gameSettings.TargetFrameRate);
        AdditiveScenesManager.Instance.LoadScenesAdditive(_gameSettings._sceneDependencys, OnAdditiveScenesLoadComplete);
    }

    private void OnAdditiveScenesLoadComplete()
    {
        EventManager.UIBus.RaiseEvent(new StartGame());
        _inMainMenu = true;
    }

    #endregion

}

public class StartGame : IEvent
{
}
