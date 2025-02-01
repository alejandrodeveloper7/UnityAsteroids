using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private SpriteRenderer _shieldFX;
    
    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
    }

    #endregion

    #region Bus Callbacks
    
    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    #endregion

    #region Initialization

    private void Initialize()
    {

    }

    #endregion
}
