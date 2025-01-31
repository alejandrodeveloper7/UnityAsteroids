using ToolsACG.Utils.Events;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private string[] _sceneDependencys;

    #endregion



    #region Monobehaviour

    private void Start()
    {
        Initialize();
    }

    #endregion

    #region Initialization

    private void Initialize()
    {
        EventManager.GetUiBus().RaiseEvent(new LoadScenesAdditive() { ScenesName = _sceneDependencys, OnComplete = OnAdditiveScenesLoadComplete });
    }

    private void OnAdditiveScenesLoadComplete()
    {
        EventManager.GetUiBus().RaiseEvent(new StartGame());
    }

    #endregion
}

public class StartGame : IEvent 
{

}
