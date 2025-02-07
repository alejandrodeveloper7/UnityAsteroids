using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PlayerGeneralController : MonoBehaviour, IPooleableGameObject
{
    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    #region Monobehavior

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus callbacks

    private async void OnPlayerDead(PlayerDead pPlayerDead) 
    {
        await Task.Delay(400);
        _originPool.RecycleItem(gameObject);
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        _originPool.RecycleItem(gameObject);
    }

    #endregion
}
