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
        Initialize();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
    }

    #endregion

    #region Initialization

    private void Initialize()
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

    #endregion
}

public class StartGame : IEvent
{

}
