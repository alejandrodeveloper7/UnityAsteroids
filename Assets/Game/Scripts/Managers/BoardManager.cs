using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private SO_Ship _selectedShip;

    private AsteroidSettings _asteroidSettings;
    private StageSettings _stageSettings;
    private List<SO_Asteroid> _initialPosibleAsteroids;
    [Space]
    private List<AsteroidScript> _currentAsteroids = new List<AsteroidScript>();

    private int _currentRound = 0;

    #region Monobehaviour

    private void Awake()
    {
        _asteroidSettings = ResourcesManager.Instance.AsteroidSettings;
        _stageSettings = ResourcesManager.Instance.StageSettings;
        _initialPosibleAsteroids = _asteroidSettings.Asteroids.Where(x => x.IsInitialAsteroid is true).ToList();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
    }

    #endregion

    #region Bus Callbacks

    private void OnStartMatch(StartMatch pStartMatch)
    {
        _currentRound = 0;
        _selectedShip = ResourcesManager.Instance.ShipSettings.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);
        CreatePlayer();
        CreateAsteroids(_stageSettings.DelayBeforeFirstRound_MS, _initialPosibleAsteroids, 2);
    }

    private void OnAsteroidDestroyed(AsteroidDestroyed pAsteroidDestroyed)
    {
        _currentAsteroids.Remove(pAsteroidDestroyed.AsteroidScript);

        if (pAsteroidDestroyed.AsteroidData.fragmentTypes.Count > 0)
            CreateAsteroids(0, pAsteroidDestroyed.AsteroidData.fragmentTypes, pAsteroidDestroyed.AsteroidData.FragmentsAmountGeneratedOnDestruction, pAsteroidDestroyed.Position, pAsteroidDestroyed.Direction);
        else
            CheckAllAsteroidsDestroyed();
    }


    #endregion

    private void CreatePlayer()
    {
        PoolsController.Instance.GetInstance(_selectedShip.PoolName);
        EventManager.GetGameplayBus().RaiseEvent(new PlayerPrepared());
    }

    private async void CreateAsteroids(int pDelay, List<SO_Asteroid> pPosibleAsteroids, int pAmount, Vector2 pOriginPosition = default, Vector2 pOriginDirection = default)
    {
        await Task.Delay(pDelay);
        for (int i = 0; i < pAmount; i++)
        {
            SO_Asteroid newAsteroidData = pPosibleAsteroids[Random.Range(0, pPosibleAsteroids.Count)];
            Vector2 newDirection = default;

            if (pOriginDirection != Vector2.zero)
            {
                float newAngleDirection = Random.Range(-newAsteroidData.BrokenAsteroidSpawnAngle, newAsteroidData.BrokenAsteroidSpawnAngle);
                newDirection = Quaternion.AngleAxis(newAngleDirection, Vector3.forward) * pOriginDirection;
            }

            CreateAsteroid(newAsteroidData, pOriginPosition, newDirection);
        }
    }

    private void CreateAsteroid(SO_Asteroid pData, Vector2 pOriginPosition = default, Vector2 pDirection = default)
    {
        AsteroidScript newAsteroid = PoolsController.Instance.GetInstance(pData.PoolName).GetComponent<AsteroidScript>();
        _currentAsteroids.Add(newAsteroid);
        newAsteroid.Initialize(pData, pOriginPosition, pDirection);
    }

    private void CheckAllAsteroidsDestroyed()
    {
        if (_currentAsteroids.Count > 0)
            return;

        Debug.Log(string.Format("Round {0} Complete", _currentRound));
        _currentRound++;
        int asteroidsToSpawn = _stageSettings.InitialAsteroids + _currentRound;
        if (asteroidsToSpawn > _stageSettings.MaxPosibleAsteroids)
            asteroidsToSpawn = _stageSettings.MaxPosibleAsteroids;

        CreateAsteroids(_stageSettings.DelayBetweenRounds_MS,_initialPosibleAsteroids, asteroidsToSpawn);
    }

}

public class PlayerPrepared : IEvent
{
}