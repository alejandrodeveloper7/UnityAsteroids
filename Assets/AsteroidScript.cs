using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class AsteroidScript : MonoBehaviour, IPooleableItem
{
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }


    private Vector2 _direction;

    [Header("References")]
    private PolygonCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private SO_Asteroid _currentData;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out BulletScript detectedBullet);
        if (detectedBullet != null)
        {
            GenerateDestructionParticles();
            EventManager.GetGameplayBus().RaiseEvent(new AsteroidDestroyed() { AsteroidScript=this, AsteroidData=_currentData, Position=transform.position, Direction= _direction });
            CleanAsteroid();
        }
    }

    public void Initialize(SO_Asteroid pData, Vector2 pPosition=default, Vector2 pDirection = default)
    {
        _currentData = pData;
        _direction = pDirection;

        int spriteRandomValue = Random.Range(0, _currentData.possibleSprites.Length);
        _spriteRenderer.sprite = _currentData.possibleSprites[spriteRandomValue];
        
        UpdateColliderShape();
        TurnDetection(true);

        if (pPosition == Vector2.zero)
            pPosition = Random.insideUnitCircle.normalized * 15;

        if (_direction == Vector2.zero)
            _direction = Random.insideUnitCircle.normalized;

        transform.position = pPosition;
        _rigidBody.velocity = _direction * _currentData.Speed * Time.fixedDeltaTime;
    }

    private void UpdateColliderShape()
    {
        if (_spriteRenderer.sprite == null)
            return;

        Sprite sprite = _spriteRenderer.sprite;
        _collider.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < _collider.pathCount; i++)
        {
            var path = new System.Collections.Generic.List<Vector2>();
            sprite.GetPhysicsShape(i, path);
            _collider.SetPath(i, path.ToArray());
        }
    }

    private void CleanAsteroid()
    {
        StopMovement();
        TurnDetection(false);
        _originPool.RecycleItem(gameObject);
    }

    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _currentData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = PoolsController.Instance.GetInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<PooledParticleSystem>().ExecuteBehaviour();
        }
    }

    public void StopMovement()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    public void TurnDetection(bool pState)
    {
        if (pState)
            _rigidBody.WakeUp();
        else
            _rigidBody.Sleep();

        _collider.enabled = pState;
    }
}

public class AsteroidDestroyed : IEvent
{
    public AsteroidScript AsteroidScript { get; set; }
    public SO_Asteroid AsteroidData { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }
}