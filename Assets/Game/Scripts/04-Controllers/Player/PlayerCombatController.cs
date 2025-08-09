using System.Linq;
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
    private SO_Ship _shipData;
    private SO_Bullet _bulletData;

    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.InputBus.AddListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.AddListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.InputBus.RemoveListener<ShootKeyStateChange>(OnShootKeyStateChange);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.RemoveListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.UIBus.RemoveListener<GameLeaved>(OnGameLeaved);
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

        _bulletData = ResourcesManager.Instance.GetScriptableObject<BulletsCollection>(ScriptableObjectKeys.BULLET_COLLECTION_KEY).Bullets.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedBulletId);
        _shipData = ResourcesManager.Instance.GetScriptableObject<ShipsCollection>(ScriptableObjectKeys.SHIP_COLLECTION_KEY).Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _bulletSpawnPoint.transform.localPosition = _shipData.BulletsSpawnPointsLocalPosition;
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

        BulletController bulletscript = FactoryManager.Instance.GetGameObjectInstance(_bulletData.PoolName).GetComponent<BulletController>();
        bulletscript.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        bulletscript.Initialize(_bulletData);
        Invoke(nameof(ShootBullet), _bulletData.BetweenBulletsTime);

        _nextShootTime = Time.time + _bulletData.BetweenBulletsTime;
    }

    #endregion
}
