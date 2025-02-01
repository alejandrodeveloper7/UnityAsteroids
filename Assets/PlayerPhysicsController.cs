using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private PolygonCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<PlayerAppearanceUpdated>(OnPlayerAppearanceUpdated);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerAppearanceUpdated>(OnPlayerAppearanceUpdated);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    #endregion

    #region Bus callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerAppearanceUpdated(PlayerAppearanceUpdated pPlayerAppearanceUpdated)
    {
        UpdateColliderShape();
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Initialize()
    {
        TurnDetection(true);
    }

    #endregion

    #region funcionality

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

    public void TurnDetection(bool pState)
    {
        if (pState)
            _rigidBody.WakeUp();
        else
            _rigidBody.Sleep();

        _collider.enabled = pState;
    }

    #endregion
}
