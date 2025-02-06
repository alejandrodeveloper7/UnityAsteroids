using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Fields

    private int _currentRound = 0;
    private List<AsteroidController> _currentAsteroids = new List<AsteroidController>();

    [Header("Data")]
    private AsteroidsConfiguration _asteroidSettings;
    private StageSettings _stageSettings;
    private SO_Ship _ShipData;
    [Space]
    private List<SO_Asteroid> _initialPosibleAsteroids;
        
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
        EventManager.GetGameplayBus().AddListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
        EventManager.GetGameplayBus().RemoveListener<StartMatch>(OnStartMatch);
        EventManager.GetGameplayBus().RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
    }

    #endregion

    #region Bus Callbacks

    private async void OnStartGame(StartGame pStartGame) 
    {
        await Task.Delay((int)(_stageSettings.DelayBeforeDecorationAsteroids*1000));
        CreateAsteroids(0, _initialPosibleAsteroids, _stageSettings.InitialAsteroids);
    }

    private void OnStartMatch(StartMatch pStartMatch)
    {
        _ShipData = ResourcesManager.Instance.ShipsConfiguration.Ships.FirstOrDefault(x => x.Id == PersistentDataManager.SelectedShipId);
        
        _currentRound = 0;

        CleanCurrentAsteroids();
        CreatePlayer();
        CreateAsteroids(_stageSettings.DelayBeforeFirstRound, _initialPosibleAsteroids, _stageSettings.InitialAsteroids);
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

    #region Initialization

    private void GetReferences() 
    {
        _asteroidSettings = ResourcesManager.Instance.AsteroidsConfiguration;
        _stageSettings = ResourcesManager.Instance.StageSettings;
        _initialPosibleAsteroids = _asteroidSettings.Asteroids.Where(x => x.IsInitialAsteroid is true).ToList();
    }

    #endregion

    #region Elements Management

    private void CreatePlayer()
    {
        PoolsManager.Instance.GetInstance(_ShipData.PoolName);
        EventManager.GetGameplayBus().RaiseEvent(new PlayerPrepared());
    }

    private async void CreateAsteroids(float pDelay, List<SO_Asteroid> pPosibleAsteroids, int pAmount, Vector2 pOriginPosition = default, Vector2 pOriginDirection = default)
    {
        await Task.Delay((int)(pDelay*1000));

        for (int i = 0; i < pAmount; i++)
        {
            SO_Asteroid newAsteroidData = pPosibleAsteroids[Random.Range(0, pPosibleAsteroids.Count)];
            Vector2 newDirection = default;

            if (pOriginDirection != Vector2.zero)
            {
                float newAngleDirection = Random.Range(-newAsteroidData.AsteroidSpawnAngle, newAsteroidData.AsteroidSpawnAngle);
                newDirection = Quaternion.AngleAxis(newAngleDirection, Vector3.forward) * pOriginDirection;
            }

            CreateAsteroid(newAsteroidData, pOriginPosition, newDirection);
        }
    }
    private void CreateAsteroid(SO_Asteroid pData, Vector2 pOriginPosition = default, Vector2 pDirection = default)
    {
        AsteroidController newAsteroid = PoolsManager.Instance.GetInstance(pData.PoolName).GetComponent<AsteroidController>();
        _currentAsteroids.Add(newAsteroid);
        newAsteroid.Initialize(pData, pOriginPosition, pDirection);
    }
    private void CleanCurrentAsteroids() 
    {
        foreach (AsteroidController item in _currentAsteroids)
            item.CleanAsteroid();

        _currentAsteroids.Clear();
    }
    private void CheckAllAsteroidsDestroyed()
    {
        if (_currentAsteroids.Count > 0)
            return;

        _currentRound++;
        Debug.Log(string.Format("- PROGRESS - Round {0} Complete", _currentRound));

        int asteroidsToSpawn = _stageSettings.InitialAsteroids + _currentRound;
        if (asteroidsToSpawn > _stageSettings.MaxPosibleAsteroids)
            asteroidsToSpawn = _stageSettings.MaxPosibleAsteroids;

        CreateAsteroids(_stageSettings.DelayBetweenRounds,_initialPosibleAsteroids, asteroidsToSpawn);
    }

    #endregion
}

public class PlayerPrepared : IEvent
{
}