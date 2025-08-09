using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    #region Fields

    [Header("Data")]
    private PlayerSettings _playerSettings;

    [Header("Stats")]
    private int _health;

    [Header("States")]
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
        EventManager.GameplayBus.AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.AddListener<PlayerHitted>(OnPlayerHitted);
        EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.RemoveListener<PlayerHitted>(OnPlayerHitted);
        EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        RestartStats();
    }

    private void OnPlayerHitted(PlayerHitted pPlayerHitted)
    {
        PlayterHitted();
    }

    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active)
            _shieldActive = true;
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _playerSettings = ResourcesManager.Instance.GetScriptableObject<PlayerSettings>(ScriptableObjectKeys.PLAYER_SETTINGS_KEY);
    }

    private void RestartStats()
    {
        _health = _playerSettings.HealthPoints;

        _isAlive = true;
        _shieldActive = true;
    }

    #endregion

    #region Functionality

    private void PlayterHitted()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            EventManager.GameplayBus.RaiseEvent(new ShieldStateChanged(_shieldActive));
            return;
        }

        _health--;
        EventManager.GameplayBus.RaiseEvent(new PlayerDamaged(_health));
        if (_isAlive && _health <= 0)
        {
            _isAlive = false;
            EventManager.GameplayBus.RaiseEvent(new PlayerDead());
        }
    }

    #endregion
}

#region IEvents

public readonly struct ShieldStateChanged : IEvent
{
    public readonly bool Active;

    public ShieldStateChanged(bool active)
    {
        Active = active;
    }
}

public readonly struct PlayerDamaged : IEvent
{
    public readonly int Health;

    public PlayerDamaged(int health)
    {
        Health = health;
    }
}

public readonly struct PlayerDead : IEvent
{
}

#endregion
