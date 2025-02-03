using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private Rigidbody2D _rigidBody;
    [SerializeField] private SpriteRenderer _propulsionFireFX;

    private SO_Ship _shipData;
    private float _movementSpeed;
    private float _rotationSpeed;
    private bool _isAlive;

    private int _rotationValue;
    private bool _movingForward;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<RotationtKeyStateChange>(OnRotationtKeyStateChange);
        EventManager.GetGameplayBus().AddListener<MoveForwardKeyStateChange>(OnMoveForwardKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PauseStateChanged>(OnPauseStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<RotationtKeyStateChange>(OnRotationtKeyStateChange);
        EventManager.GetGameplayBus().RemoveListener<MoveForwardKeyStateChange>(OnMoveForwardKeyStateChange);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PauseStateChanged>(OnPauseStateChanged);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnRotationtKeyStateChange(RotationtKeyStateChange pRotationtKeyStateChange)
    {
        _rotationValue = pRotationtKeyStateChange.Value;

        if (_rotationValue != 0)
            RotateAround();
    }
    private void OnMoveForwardKeyStateChange(MoveForwardKeyStateChange pMoveForwardKeyStateChange)
    {
        _movingForward = pMoveForwardKeyStateChange.State;
        UpdatePropulsionFire();

        if (_movingForward)
            MoveForward();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _isAlive = false;
        _rotationValue = 0;
        _movingForward = false;
    }

    private void OnPauseStateChanged(PauseStateChanged pPauseStateChanged)
    {
        if (pPauseStateChanged.InPause)
        {
            _rotationValue = 0;
            _movingForward = false;
        }
    }
    #endregion

    #region Initilization

    private void GetReferences()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Initialize()
    {
        _isAlive = true;
        transform.SetPositionAndRotation(Vector2.zero, Quaternion.Euler(0, 0, 180));

        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);
        _movementSpeed = _shipData.movementSpeed;
        _rotationSpeed = _shipData.rotationSpeed;
        _propulsionFireFX.transform.localPosition = _shipData.PropulsionFireLocalPosition;
        _propulsionFireFX.color = new Color(_propulsionFireFX.color.r, _propulsionFireFX.color.g, _propulsionFireFX.color.b, 0f);
    }

    #endregion

    #region Funcionality

    private void UpdatePropulsionFire()
    {
        _propulsionFireFX.DOKill();

        if (_movingForward)
            _propulsionFireFX.DOFade(1, 0.5f);
        else
            _propulsionFireFX.DOFade(0, 0.5f);
    }

    public void StopMovement()
    {
        _rigidBody.velocity = Vector2.zero;
    }

    public async void MoveForward()
    {
        if (_isAlive is false)
            return;

        while (_movingForward)
        {
            _rigidBody.AddForce(-transform.up * _movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
            await Task.Yield();
        }
    }

    public async void RotateAround()
    {
        if (_isAlive is false)
            return;

        while (_rotationValue != 0)
        {
            transform.Rotate(0, 0, _rotationValue * _rotationSpeed * Time.fixedDeltaTime);
            await Task.Yield();
        }
    }

    #endregion

}
