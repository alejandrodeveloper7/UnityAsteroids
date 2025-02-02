using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    #region Fields
    private PlayerSettings _playerSettings;
    [SerializeField] private SpriteRenderer _shieldFX;

    private int _health;
    private bool _shieldActive;
    private bool _isAlive;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<PlayerHitted>(OnPlayerHitted);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerHitted>(OnPlayerHitted);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerHitted(PlayerHitted pPlayerHitted)
    {
        _health--;
        EventManager.GetGameplayBus().RaiseEvent(new PlayerHealthUpdated() { Health = _health });
        if (_isAlive && _health <= 0) 
        {
            _isAlive = false;
            EventManager.GetGameplayBus().RaiseEvent(new PlayerDead());
        }

    }

    #endregion

    #region Initialization

    private void Initialize()
    {
        _health = _playerSettings.HealthPoints;
        _isAlive = true;
    }

    private void GetReferences()
    {
        _playerSettings = ResourcesManager.Instance.PlayerSettings;
    }

    #endregion
}

public class PlayerHealthUpdated : IEvent
{
    public int Health { get; set; }
}

public class PlayerDead : IEvent
{
}
