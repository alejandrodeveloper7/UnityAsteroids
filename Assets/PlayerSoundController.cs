using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    private SO_Ship _shipData;

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().AddListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().AddListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GetGameplayBus().AddListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
        EventManager.GetGameplayBus().RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.GetGameplayBus().RemoveListener<PlayerDamaged>(OnPlayerDamaged);
        EventManager.GetGameplayBus().RemoveListener<ShieldStateChanged>(OnShieldStateChanged);
    }

    private void Initialize()
    {
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);
    }

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    private void OnPlayerDead(PlayerDead pPlayerDead)
    {
        EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _shipData.SoundsOnDestruction });
    }

    private void OnPlayerDamaged(PlayerDamaged pPlayerDamaged)
    {
        EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _shipData.SoundsOnDamage });
    }

    private void OnShieldStateChanged(ShieldStateChanged pShieldStateChanged) 
    {
        if(pShieldStateChanged.Active)
            EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _shipData.SoundsOnShieldUp });
        else
            EventManager.GetGameplayBus().RaiseEvent(new GenerateSound() { SoundsData = _shipData.SoundsOnShieldDown });
    }

}