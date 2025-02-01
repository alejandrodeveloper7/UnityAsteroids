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
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<ShootKeyStateChange>(OnShootKeyStateChange);
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
            BulletScript bulletscript = PoolsController.Instance.GetInstance(_bulletData.PoolName).GetComponent<BulletScript>();
            bulletscript.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            bulletscript.Initialize(_bulletData);
            await Task.Delay((int)(_bulletData.BetweenBulletsTime * 1000));
        }
    }
}
