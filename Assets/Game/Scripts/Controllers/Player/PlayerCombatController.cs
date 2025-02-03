using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    #region Fields

    private SO_Ship _shipData;
    private SO_Bullet _bulletData;

    [SerializeField] private Transform _bulletSpawnPoint;

    private bool _isAlive;
    private bool _shooting;
    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PauseStateChanged>(OnPauseStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PauseStateChanged>(OnPauseStateChanged);
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

    #endregion

    #region Initialization

    private void Initialize()
    {
        _isAlive = true;

        _bulletData = ResourcesManager.Instance.BulletSettings.Bullets.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedBulletId);
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _bulletSpawnPoint.transform.localPosition = _shipData.BulletsSpawnPointsLocalPosition;
    }

    #endregion

    public void ShootBullet()
    {
        if (_isAlive is false)
            return;
        if (_shooting is false)
            return;

        BulletController bulletscript = PoolsManager.Instance.GetInstance(_bulletData.PoolName).GetComponent<BulletController>();
        bulletscript.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        bulletscript.Initialize(_bulletData);
        Invoke(nameof(ShootBullet), _bulletData.BetweenBulletsTime);
    }
}
