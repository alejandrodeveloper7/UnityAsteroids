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
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
    }

    private async void OnPlayerDead(PlayerDead pPlayerDead) 
    {
        await Task.Delay(400);
        _originPool.RecycleItem(gameObject);
    }
}
