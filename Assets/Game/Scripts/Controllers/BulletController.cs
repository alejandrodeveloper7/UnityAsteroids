using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class BulletController : MonoBehaviour, IPooleableItem
{
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }


    [Header("References")]
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private SO_Bullet _currentData;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out AsteroidController detectedAsteroid);
        if (detectedAsteroid != null)
        {
            CancelInvoke(nameof(CleanBullet));
            GenerateDestructionParticles();
            CleanBullet();
        }
    }

    public void Initialize(SO_Bullet pData)
    {
        _currentData = pData;

        _spriteRenderer.sprite = _currentData.Sprite;
        EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _currentData.SoundsOnShoot });
        TurnDetection(true);
        _rigidBody.velocity = _currentData.Speed * Time.fixedDeltaTime * -transform.up;
        Invoke(nameof(CleanBullet), _currentData.LifeDuration);
    }


    public void StopMovement()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    public void TurnDetection(bool pState)
    {
        _collider.enabled = pState;
    }

    private void CleanBullet()
    {
        StopMovement();
        TurnDetection(false);
        _originPool.RecycleItem(gameObject);
    }

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _currentData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = PoolsManager.Instance.GetInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
        }
    }
}
