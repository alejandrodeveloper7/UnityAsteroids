using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private SpriteRenderer _spriteRenderer;

    private SO_Ship _shipData;

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
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
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


    #endregion

    #region Initialization

    private void GetReferences()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Initialize()
    {
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _spriteRenderer.sprite = _shipData.Sprite;
        EventManager.GetGameplayBus().RaiseEvent(new PlayerAppearanceUpdated());
    }

    #endregion

    private void PlayerDead() 
    {
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
