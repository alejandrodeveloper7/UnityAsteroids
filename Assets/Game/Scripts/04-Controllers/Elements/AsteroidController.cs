using System.Collections.Generic;
using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IPooleableItem
{
    #region Fields
    
    [Header("IPooleableItem")]
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private PolygonCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private ScreenEdgeTeleport _screenEdgeTeleporter;
    private Destructible _destructible;

    [Header("Data")]
    private SO_Asteroid _asteroidData;
    private Vector2 _direction;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }   

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _screenEdgeTeleporter = GetComponent<ScreenEdgeTeleport>();
        _destructible = GetComponent<Destructible>();
    }

    public void Initialize(SO_Asteroid pData, Vector2 pPosition = default, Vector2 pDirection = default)
    {
        _asteroidData = pData;
        _direction = pDirection;

        _destructible.OnDestroyed += AsteroidDestroyed;

        int spriteRandomValue = Random.Range(0, _asteroidData.possibleSprites.Length);
        _spriteRenderer.sprite = _asteroidData.possibleSprites[spriteRandomValue];

        UpdateColliderShape();
        TurnDetection(true);

        if (pPosition == Vector2.zero)
            pPosition = GetRandomSpawnPosition();

        transform.position = pPosition;

        if (_direction == Vector2.zero)
            _direction = Random.insideUnitCircle.normalized;

        _rigidBody.velocity = _direction * _asteroidData.Speed * Time.fixedDeltaTime;
        _rigidBody.AddTorque(Random.Range(-_asteroidData.PosibleTorque, _asteroidData.PosibleTorque));
        
        _screenEdgeTeleporter.EdgeOffsetX = _asteroidData.EdgeOffsetX;
        _screenEdgeTeleporter.EdgeRepositionOffsetX = _asteroidData.EdgeRepositionOffsetX;

        _screenEdgeTeleporter.EdgeOffsetY = _asteroidData.EdgeOffsetY;
        _screenEdgeTeleporter.EdgeRepositionOffsetY = _asteroidData.EdgeRepositionOffsetY;
    }

    #endregion    

    #region Movement and Detection

    private void UpdateColliderShape()
    {
        if (_spriteRenderer.sprite == null)
            return;

        Sprite sprite = _spriteRenderer.sprite;
        _collider.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < _collider.pathCount; i++)
        {
            List<Vector2> path = new List<Vector2>();
            sprite.GetPhysicsShape(i, path);
            _collider.SetPath(i, path.ToArray());
        }
    }
    private void TurnDetection(bool pState)
    {
        if (pState)
            _rigidBody.WakeUp();
        else
            _rigidBody.Sleep();

        _collider.enabled = pState;
    }

    private void StopMovement()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    #endregion

    #region Functionality
    
    private Vector2 GetRandomSpawnPosition()
    {
        float minLimitX = 0f - _asteroidData.EdgeRepositionOffsetX;
        float maxLimitX = 1f + _asteroidData.EdgeRepositionOffsetX;
        float minLimitY = 0f - _asteroidData.EdgeRepositionOffsetY;
        float maxLimitY = 1f + _asteroidData.EdgeRepositionOffsetY;

        float RandomValueX = Random.Range(minLimitX, maxLimitX);
        float RandomValueY = Random.Range(minLimitY, maxLimitY);

        bool spawnOnXEdge = Random.value > 0.5f;

        if (spawnOnXEdge)
            return Camera.main.ViewportToWorldPoint(new Vector2(maxLimitX, RandomValueY));
        else
            return Camera.main.ViewportToWorldPoint(new Vector2(RandomValueX, maxLimitY));
    }
    
    private void GenerateDestructionParticles()
    {
        foreach (ParticleSetup item in _asteroidData.DestuctionParticles)
        {
            ParticleSystem pooledParticlesystem = PoolsManager.Instance.GetInstance(item.particleEffectName).GetComponent<ParticleSystem>();
            if (item.particleConfig != null)
                item.particleConfig.ApplyConfig(pooledParticlesystem);
            pooledParticlesystem.transform.position = transform.position;
            pooledParticlesystem.GetComponent<ParticleSystemController>().Play();
            EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _asteroidData.SoundsOnDestruction });
        }
    }

    private void AsteroidDestroyed()
    {
        GenerateDestructionParticles();
        EventManager.GetGameplayBus().RaiseEvent(new AsteroidDestroyed() { AsteroidScript = this, AsteroidData = _asteroidData, Position = transform.position, Direction = _direction });
        CleanAsteroid();
    }

    public void CleanAsteroid()
    {
        _destructible.OnDestroyed -= AsteroidDestroyed;

        StopMovement();
        TurnDetection(false);
        _originPool.RecycleItem(gameObject);
    }
    
    #endregion
}

public class AsteroidDestroyed : IEvent
{
    public AsteroidController AsteroidScript { get; set; }
    public SO_Asteroid AsteroidData { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }
}