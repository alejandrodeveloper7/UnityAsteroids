using System.Linq;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    #region Fields

    private SO_Ship _shipData;
    private SO_Bullet _bulletData;

    [SerializeField] private Transform _bulletSpawnPoint;

    private bool _shooting;
    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PauseKeyClicked>(OnPauseKeyClicked);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PauseKeyClicked>(OnPauseKeyClicked);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnShootKeyStateChange(ShootKeyStateChange pShootKeyStateChange)
    {
        _shooting = pShootKeyStateChange.State;

        if (_shooting)
            ShotBullet();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        _shooting = false;
    }

    private void OnPauseKeyClicked(PauseKeyClicked pPauseKeyClicked)
    {
        if (pPauseKeyClicked.InPause)
            _shooting = false;
    }

    #endregion

    #region Initialization

    private void Initialize()
    {
        _bulletData = ResourcesManager.Instance.BulletSettings.Bullets.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedBulletId);
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _bulletSpawnPoint.transform.localPosition = _shipData.BulletsSpawnPointsLocalPosition;
    }

    #endregion

    public async void ShotBullet()
    {
        while (_shooting)
        {
            BulletController bulletscript = PoolsManager.Instance.GetInstance(_bulletData.PoolName).GetComponent<BulletController>();
            bulletscript.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            bulletscript.Initialize(_bulletData);
            await Task.Delay((int)(_bulletData.BetweenBulletsTime * 1000));
        }
    }
}
