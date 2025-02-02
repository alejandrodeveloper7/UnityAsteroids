using ToolsACG.Utils.Events;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private string[] _sceneDependencys;

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
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().AddListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().RemoveListener<BackToMenuButtonClicked>(OnBackToMenuButtonClicked);
    }

    #endregion

    #region Initialization

    private void InitializeGame()
    {
        EventManager.GetUiBus().RaiseEvent(new LoadScenesAdditive() { ScenesName = _sceneDependencys, OnComplete = OnAdditiveScenesLoadComplete });
    }

    private void OnAdditiveScenesLoadComplete()
    {
        _inMainMenu = true;
        EventManager.GetUiBus().RaiseEvent(new StartGame());
    }

    #endregion

    #region Bus Callbacks

    private void OnStartMatch(StartMatch pStartMatch)
    {
        _inMainMenu = false;
        _inMatch = true;
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _inMatch = false;
        _inLeaderboard = true;
    }

    private void OnBackToMenuButtonClicked(BackToMenuButtonClicked pBackToMenuButtonClicked) 
    {
        _inLeaderboard = false;
        _inMainMenu = true;
    }

    #endregion
}

public class StartGame : IEvent
{

}
