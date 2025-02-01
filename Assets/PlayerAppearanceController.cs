using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private SpriteRenderer _spriteRenderer;

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
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PlayerPrepared>(OnPlayerPrepared);
    }

    #endregion

    #region Bus Callbacks

    private void OnPlayerPrepared(PlayerPrepared pPlayerPrepared)
    {
        Initialize();
    }

    #endregion

    #region Initialization
    
    private void GetReferences()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
     
    private void Initialize()
    {
        _shipData = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);

        _spriteRenderer.sprite = _shipData.Sprite;
        EventManager.GetGameplayBus().RaiseEvent(new PlayerAppearanceUpdated());
    }

    #endregion
}

public class PlayerAppearanceUpdated : IEvent 
{
}
