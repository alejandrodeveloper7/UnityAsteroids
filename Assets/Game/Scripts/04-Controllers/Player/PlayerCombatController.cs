using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Transform _bulletSpawnPoint;

    [Header("States")]
    private bool _isAlive;
    private bool _shooting;
    [Space]
    private float _nextShootTime;
    
    [Header("Data")]
    private SO_Ship _shipsConfiguration;
    private SO_Bullet _bulletData;

    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GetUiBus().RemoveListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnShootKeyStateChange(ShootKeyStateChange pShootKeyStateChange)
    {
        CancelInvoke(nameof(ShootBullet));
        _shooting = pShootKeyStateChange.State;

        if (_shooting)
            ShootBullet();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _isAlive = false;
        _shooting = false;
    }

    private void OnPauseStateChanged(PauseStateChanged pPauseStateChanged)
    {
        if (pPauseStateChanged.InPause)
            _shooting = false;
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        _isAlive = false;
        _shooting = false;
    }

    #endregion

    #region Initialization

    private void Initialize()
    {
        _isAlive = true;

        _bulletData = ResourcesManager.Instance.BulletsConfiguration.Bullets.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedBulletId);
        _shipsConfiguration = ResourcesManager.Instance.ShipsConfiguration.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _bulletSpawnPoint.transform.localPosition = _shipsConfiguration.BulletsSpawnPointsLocalPosition;
    }

    #endregion

    #region Functionality

    public void ShootBullet()
    {
        if (_isAlive is false)
            return;
        if (_shooting is false)
            return;

        if (Time.time < _nextShootTime) 
        {
            Invoke(nameof(ShootBullet), _nextShootTime - Time.time);
            return;
        }

        BulletController bulletscript = PoolsManager.Instance.GetGameObjectInstance(_bulletData.PoolName).GetComponent<BulletController>();
        bulletscript.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        bulletscript.Initialize(_bulletData);
        Invoke(nameof(ShootBullet), _bulletData.BetweenBulletsTime);

        _nextShootTime = Time.time + _bulletData.BetweenBulletsTime;
    }

    #endregion
}
