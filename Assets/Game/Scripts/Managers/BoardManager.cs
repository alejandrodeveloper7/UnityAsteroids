using System.Collections.Generic;
using System.Linq;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private AsteroidSettings _asteroidSettings;
    private ShipSettings _shipSettings;
    [Space]
    private int _selectedShipId;

    private PlayerScript _playerScript;
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
        _playerScript = PoolsController.Instance.GetInstance(_shipSettings.PoolName).GetComponent<PlayerScript>();
        _playerScript.Initialize(_shipSettings.Ships.FirstOrDefault(x => x.Id == _selectedShipId));
   }

    private void CreateAsteroids() 
    {
    
    }

}
