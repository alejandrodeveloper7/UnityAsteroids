using DG.Tweening;
using System.Linq;
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
        EventManager.GameplayBus.AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
        EventManager.GameplayBus.AddListener<PlayerHitted>(OnPlayerHitted);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
        EventManager.GameplayBus.RemoveListener<PlayerHitted>(OnPlayerHitted);
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
        DisplayInvulnerabilityBlinkSequence();
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
        _shipData = ResourcesManager.Instance.GetScriptableObject<ShipsCollection>(ScriptableObjectKeys.SHIP_COLLECTION_KEY).Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _shipSpriteRenderer.sprite = _shipData.Sprite;
        _shieldFX.enabled = true;
        _shieldFX.color = _shipData.ShieldColor;
        StartShieldBlink();

        _CollisionCooldownSequence?.Kill();
        _shipSpriteRenderer.color = _storedColor;

        EventManager.GameplayBus.RaiseEvent(new PlayerAppearanceUpdated());
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
            StartShieldBlink();
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

    private void StartShieldBlink()
    {
        _blinkSequence = DOTween.Sequence()
        .Append(_shieldFX.DOFade(_shipData.BlinkMinAlpha, _shipData.BlickDuration).SetEase(Ease.InOutSine))
        .Append(_shieldFX.DOFade(_shipData.BlinkMaxAlpha, _shipData.BlickDuration).SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo);
    }

    #endregion

    #region Invulnerability

    private void DisplayInvulnerabilityBlinkSequence() 
    {
        if (_CollisionCooldownSequence != null)
            _CollisionCooldownSequence?.Kill();

        _CollisionCooldownSequence = DOTween.Sequence()
                .Append(_shipSpriteRenderer.DOFade(0f,(float)1/(_shipData.InvulnerabilityBlinksPerSecond*2)).SetEase(Ease.Linear))
                .Append(_shipSpriteRenderer.DOFade(1f, (float)1 / (_shipData.InvulnerabilityBlinksPerSecond*2)).SetEase(Ease.Linear))
                .SetLoops((int)_shipData.InvulnerabilityDuration* _shipData.InvulnerabilityBlinksPerSecond, LoopType.Yoyo);
    }

    #endregion

    #region Particles

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _shipData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = FactoryManager.Instance.GetGameObjectInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
        }
    }
    
    #endregion
}

public readonly struct PlayerAppearanceUpdated : IEvent
{
}
