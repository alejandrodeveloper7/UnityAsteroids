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

    public void Initialize(SO_Asteroid pData, Vector2 pDirection = default)
    {
        _currentData = pData;

        int spriteRandomValue = Random.Range(0, _currentData.possibleSprites.Length);
        _spriteRenderer.sprite = _currentData.possibleSprites[spriteRandomValue];
        UpdateColliderShape();

        if (pDirection == Vector2.zero)
            pDirection = Random.insideUnitCircle.normalized;

        transform.position = new Vector2(Random.Range(-6, 6), Random.Range(-6, 6));

        TurnAsteroidDetection(true);
        //_rigidBody.velocity = pDirection * _currentData.Speed * Time.fixedDeltaTime;
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

    public void StopAsteroid()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    public void TurnAsteroidDetection(bool pState)
    {
        if (pState)
            _rigidBody.WakeUp();
        else
            _rigidBody.Sleep();

        _collider.enabled = pState;
    }
}
