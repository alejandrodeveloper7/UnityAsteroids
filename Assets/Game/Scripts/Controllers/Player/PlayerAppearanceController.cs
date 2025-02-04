using DG.Tweening;
using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private SpriteRenderer _shipSpriteRenderer;
    private SO_Ship _shipData;

    [SerializeField] private SpriteRenderer _shieldFX;
    private Sequence _blinkSequence;
    private Sequence _CollisionCooldownSequence;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<ShieldStateChanged>(OnShieldStateChanged);
        EventManager.GetGameplayBus().AddListener<PlayerHitted>(OnPlayerHitted);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
        EventManager.GetGameplayBus().AddListener<PlayerHitted>(OnPlayerHitted);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        PlayerDead();
    }

    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active)
            ActivateShield();
        else
            DeactivateShield();
    }

    private void OnPlayerHitted(PlayerHitted pPlayerHitted)
    {
        _CollisionCooldownSequence?.Kill();
        _CollisionCooldownSequence = DOTween.Sequence()
                .Append(_shipSpriteRenderer.DOFade(0f, 0.25f).SetEase(Ease.Linear))
                .Append(_shipSpriteRenderer.DOFade(1f, 0.25f).SetEase(Ease.Linear))
                .SetLoops(10, LoopType.Yoyo);
                
    }

    #endregion

    public void ActivateShield()
    {
        _shieldFX.enabled = true;

        if (_blinkSequence != null)
            _blinkSequence.Kill();
        _shieldFX.DOFade(_shipData.BlinkMaxAlpha, _shipData.FadeInDuration).OnComplete(() =>
        {
            StartBlink();
        });
    }

    public void DeactivateShield()
    {
        if (_blinkSequence != null)
            _blinkSequence?.Kill();

        _shieldFX.DOFade(0f, _shipData.FadeOutDuration).OnComplete(() =>
        {
            _shieldFX.enabled = false;
        });
    }

    private void StartBlink()
    {
        _blinkSequence = DOTween.Sequence()
        .Append(_shieldFX.DOFade(_shipData.BlinkMinAlpha, _shipData.BlickDuration).SetEase(Ease.InOutSine))
        .Append(_shieldFX.DOFade(_shipData.BlinkMaxAlpha, _shipData.BlickDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo);
    }

    #region Initialization

    private void GetReferences()
    {
        _shipSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Initialize()
    {
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _shipSpriteRenderer.sprite = _shipData.Sprite;
        _shieldFX.enabled = true;
        _shieldFX.color = _shipData.ShieldColor;
        StartBlink();

        _CollisionCooldownSequence?.Kill();
        _shipSpriteRenderer.color = Color.white;

        EventManager.GetGameplayBus().RaiseEvent(new PlayerAppearanceUpdated());
    }

    #endregion

    private void PlayerDead()
    {
        _CollisionCooldownSequence?.Kill();
        GenerateDestructionParticles();
    }

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _shipData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = PoolsManager.Instance.GetInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
        }
    }
}

public class PlayerAppearanceUpdated : IEvent
{
}
