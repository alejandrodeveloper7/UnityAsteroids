using ToolsACG.Utils.Pooling;
using UnityEngine;

public class BulletController : MonoBehaviour, IPooleableGameObject
{
    #region Fields

    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private ScreenEdgeTeleport _screenEdgeTeleporter;

    [Header("Data")]
    private SO_Bullet _bulletData;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out IDamageable detectedDamageable);

        if (detectedDamageable != null)
        {
            detectedDamageable.TakeDamage(_bulletData.Damage);

            CancelInvoke(nameof(CleanBullet));
            GenerateDestructionParticles();
            CleanBullet();
        }
    }

    #endregion

    #region Bus callbacks

    private void OnGameLeaved(GameLeaved pGameLeaved)
    {
        CancelInvoke(nameof(CleanBullet));
        CleanBullet();
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _screenEdgeTeleporter = GetComponent<ScreenEdgeTeleport>();
    }

    public void Initialize(SO_Bullet pData)
    {
        _bulletData = pData;

        _spriteRenderer.sprite = _bulletData.Sprite;
        _spriteRenderer.color = _bulletData.Color;
        _rigidBody.velocity = _bulletData.Speed * Time.fixedDeltaTime * -transform.up;

        TurnDetection(true);

        _screenEdgeTeleporter.EdgeOffsetX = _bulletData.EdgeOffsetX;
        _screenEdgeTeleporter.EdgeRepositionOffsetX = _bulletData.EdgeRepositionOffsetX;

        _screenEdgeTeleporter.EdgeOffsetY = _bulletData.EdgeOffsetY;
        _screenEdgeTeleporter.EdgeRepositionOffsetY = _bulletData.EdgeRepositionOffsetY;

        EventManager.SoundBus.RaiseEvent(new Generate2DSound(_bulletData.SoundsOnShoot));
        Invoke(nameof(CleanBullet), _bulletData.LifeDuration);
    }

    #endregion

    #region Functionality

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _bulletData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = FactoryManager.Instance.GetGameObjectInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);

            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
        }
    }

    private void StopMovement()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    private void TurnDetection(bool pState)
    {
        _collider.enabled = pState;
    }

    private void CleanBullet()
    {
        StopMovement();
        TurnDetection(false);
        _originPool.RecycleGameObject(gameObject);
    }

    #endregion
}
