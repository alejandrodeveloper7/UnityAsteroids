using System.Collections.Generic;
using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private PolygonCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    [Header("States")]
    private bool _inCollisionCooldown;

    [Header("Data")]
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
        EventManager.GetGameplayBus().AddListener<PlayerAppearanceUpdated>(OnPlayerAppearanceUpdated);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerAppearanceUpdated>(OnPlayerAppearanceUpdated);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetUiBus().RemoveListener<GameLeaved>(OnGameLeaved);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_inCollisionCooldown)
            return;

        collision.TryGetComponent(out AsteroidController detectedAsteroid);
        if (detectedAsteroid != null)
        {
            EventManager.GetGameplayBus().RaiseEvent(new PlayerHitted());
            _inCollisionCooldown = true;
            Invoke(nameof(ResetCollisionCooldown), _shipData.InvulnerabilityDuration);
        }
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

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        CancelInvoke(nameof(ResetCollisionCooldown));
        TurnDetection(false);
    }

    private void OnGameLeaved(GameLeaved opGameLeaved)
    {
        CancelInvoke(nameof(ResetCollisionCooldown));
        TurnDetection(false);
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
        _shipData = ResourcesManager.Instance.ShipsConfiguration.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _inCollisionCooldown = false;
        TurnDetection(true);
    }

    #endregion

    #region Physics Management

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

    public void TurnDetection(bool pState)
    {
        if (pState)
            _rigidBody.WakeUp();
        else
            _rigidBody.Sleep();

        _collider.enabled = pState;
    }

    void ResetCollisionCooldown()
    {
        _inCollisionCooldown = false;
    }

    #endregion
}

public class PlayerHitted : IEvent
{
}
