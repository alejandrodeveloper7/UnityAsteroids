using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PlayerGeneralController : MonoBehaviour, IPooleableItem
{
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

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

    private async void OnPlayerDead(PlayerDead pPlayerDead) 
    {
        await Task.Delay(400);
        _originPool.RecycleItem(gameObject);
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        _originPool.RecycleItem(gameObject);
    }
}
