using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    #region Fields

    private PlayerSettings _playerSettings;

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
        EventManager.GetGameplayBus().AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerHitted>(OnPlayerHitted);
        EventManager.GetGameplayBus().RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerHitted(PlayerHitted pPlayerHitted)
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            EventManager.GetGameplayBus().RaiseEvent(new ShieldStateChanged() { Active = _shieldActive });
            return;
        }

        _health--;
        EventManager.GetGameplayBus().RaiseEvent(new PlayerDamaged() { Health = _health });
        if (_isAlive && _health <= 0)
        {
            _isAlive = false;
            EventManager.GetGameplayBus().RaiseEvent(new PlayerDead());
        }
    }

    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active)
            _shieldActive = true;
    }

    #endregion



    #region Initialization

    private void Initialize()
    {
        _health = _playerSettings.HealthPoints;

        _isAlive = true;
        _shieldActive = true;
    }

    private void GetReferences()
    {
        _playerSettings = ResourcesManager.Instance.PlayerSettings;
    }

    #endregion
}

public class ShieldStateChanged : IEvent
{
    public bool Active { get; set; }
}

public class PlayerDamaged : IEvent
{
    public int Health { get; set; }
}

public class PlayerDead : IEvent
{
}
