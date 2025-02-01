using System.Collections.Generic;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private AsteroidSettings _asteroidSettings;
    private ShipSettings _shipSettings;
    [Space]
    private int _selectedShipId;

    private List<AsteroidScript> _currentasteroids;

    #region Monobehaviour

    private void Awake()
    {
        _asteroidSettings = ResourcesManager.Instance.AsteroidSettings;
        _shipSettings = ResourcesManager.Instance.ShipSettings;
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
    }

    #endregion

    #region Bus Callbacks

    private void OnStartMatch (StartMatch pStartMatch) 
    {
        _selectedShipId = PersistentDataManager.SelectedShipId;
        CreatePlayer();
        CreateAsteroids();
    }

    #endregion

    private void CreatePlayer() 
    {
        PoolsController.Instance.GetInstance(_shipSettings.PoolName);
        EventManager.GetGameplayBus().RaiseEvent(new PlayerPrepared());
    }

    private void CreateAsteroids() 
    {
    
    }

}

public class PlayerPrepared : IEvent 
{
}