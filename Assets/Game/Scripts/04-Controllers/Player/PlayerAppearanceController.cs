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

    [Header("DoTween")]
    private Sequence _blinkSequence;
    private Sequence _CollisionCooldownSequence;


    private Color _storedColor;
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
        EventManager.GetGameplayBus().RemoveListener<PlayerHitted>(OnPlayerHitted);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _CollisionCooldownSequence?.Kill();
        GenerateDestructionParticles();
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
        InvulnerabilitySequence();
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _shipSpriteRenderer = GetComponent<SpriteRenderer>();
        _storedColor = _shipSpriteRenderer.color;
    }

    private void Initialize()
    {
        _shipData = ResourcesManager.Instance.ShipsConfiguration.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _shipSpriteRenderer.sprite = _shipData.Sprite;
        _shieldFX.enabled = true;
        _shieldFX.color = _shipData.ShieldColor;
        StartBlink();

        _CollisionCooldownSequence?.Kill();
        _shipSpriteRenderer.color = _storedColor;

        EventManager.GetGameplayBus().RaiseEvent(new PlayerAppearanceUpdated());
    }

    #endregion

    #region Shield Management

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

    #endregion

    #region Invulnerability

    private void InvulnerabilitySequence() 
    {
        if (_CollisionCooldownSequence != null)
            _CollisionCooldownSequence?.Kill();

        _CollisionCooldownSequence = DOTween.Sequence()
                .Append(_shipSpriteRenderer.DOFade(0f, 0.25f).SetEase(Ease.Linear))
                .Append(_shipSpriteRenderer.DOFade(1f, 0.25f).SetEase(Ease.Linear))
                .SetLoops((int)_shipData.InvulnerabilityDuration*2, LoopType.Yoyo);
    }

    #endregion

    #region Particles

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _shipData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = PoolsManager.Instance.GetGameObjectInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
        }
    }
    
    #endregion
}

public class PlayerAppearanceUpdated : IEvent
{
}
