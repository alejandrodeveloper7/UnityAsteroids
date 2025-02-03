using DG.Tweening;
using System.Linq;
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

    private Sequence _blinkSequence;

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
            DeactivateShield();
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
        if (pShieldStateChanged.Active is false)
            return;

        _shieldActive = true;
        ActivateShield();
    }

    #endregion

    public void ActivateShield()
    {
        _shieldFX.enabled = true;

        if (_blinkSequence != null)
            _blinkSequence.Kill();

        _shieldFX.DOFade(_playerSettings.BlinkMaxAlpha, _playerSettings.FadeInDuration).OnComplete(() =>
        {
            StartBlink();
        });
    }

    public void DeactivateShield()
    {
        if (_blinkSequence != null)
            _blinkSequence?.Kill();

        _shieldFX.DOFade(0f, _playerSettings.FadeOutDuration).OnComplete(() =>
        {
            _shieldFX.enabled = false;
        });
    }

    private void StartBlink()
    {
        _blinkSequence = DOTween.Sequence()
            .Append(_shieldFX.DOFade(_playerSettings.BlinkMinAlpha, _playerSettings.BlickDuration).SetEase(Ease.InOutSine))
            .Append(_shieldFX.DOFade(_playerSettings.BlinkMaxAlpha, _playerSettings.BlickDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void SetShieldAlpha(float pAlpha)
    {
        var color = _shieldFX.color;
        color.a = pAlpha;
        _shieldFX.color = color;
    }

    #region Initialization

    private void Initialize()
    {
        _health = _playerSettings.HealthPoints;
        _isAlive = true;

        _shieldActive = true;
        _shieldFX.enabled = true;
        _shieldFX.color =_playerSettings.ShieldColor;
        StartBlink();
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
