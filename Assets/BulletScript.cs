using ToolsACG.Utils.Pooling;
using UnityEngine;

public class BulletScript : MonoBehaviour,IPooleableItem
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

    public void Initialize(SO_Bullet pData)
    {
        _currentData = pData;

        _spriteRenderer.sprite = _currentData.Sprite;
     
        TurnBulletDetection(true);
        //_rigidBody.velocity = pDirection * _currentData.Speed * Time.fixedDeltaTime;
    }


    public void StopBullet()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    public void TurnBulletDetection(bool pState)
    {
        _collider.enabled = pState;
    }
}
