using System.Linq;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    private SO_Ship _shipData;

    #region Monobehaviour

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.AddListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GameplayBus.AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GameplayBus.RemoveListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GameplayBus.RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    #endregion

    #region Bus callbacks 

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        _shipData = ResourcesManager.Instance.GetScriptableObject<ShipsCollection>(ScriptableObjectKeys.SHIP_COLLECTION_KEY).Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        EventManager.SoundBus.RaiseEvent(new Generate2DSound(_shipData.SoundsOnDestruction));
    }

    private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
    {
        EventManager.SoundBus.RaiseEvent(new Generate2DSound(_shipData.SoundsOnDamage));
    }

    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged)
    {
        if (pShieldStateChanged.Active)
            EventManager.SoundBus.RaiseEvent(new Generate2DSound(_shipData.SoundsOnShieldUp));
        else
            EventManager.SoundBus.RaiseEvent(new Generate2DSound(_shipData.SoundsOnShieldDown));
    }

    #endregion
}