using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Core.ScriptableObjects.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Gameplay.Asteroids.Controllers;
using Asteroids.Gameplay.Asteroids.Spawners;
using Zenject;
using ACG.Core.EventBus;
using ACG.Scripts.Services;
using ACG.Core.Utils;

namespace Asteroids.Core.Controllers
{
    public class AsteroidsController
    {
        #region Fields

        [Header("References")]
        private readonly AsteroidSpawner _asteroidSpawner;
        [Space]
        private readonly IDebugService _debugService;

        [Header("Values")]
        private readonly List<AsteroidController> _currentAsteroids = new();
        private int _currentRound = 0;

        [Header("data")]
        private readonly SO_StageConfiguration _stageConfiguration;

        #endregion

        #region Constructors and Dispose

        [Inject]
        public AsteroidsController(AsteroidSpawner asteroidSpawner, SO_StageConfiguration stageConfiguration, IDebugService debugService)
        {
            _asteroidSpawner = asteroidSpawner;
            _stageConfiguration = stageConfiguration;
            _debugService = debugService;

            EventBusManager.GameplayBus.AddListener<AsteroidDestroyed>(OnAsteroidDestroyed);
        }

        public void Dispose()
        {
            CleanCurrentAsteroids();

            EventBusManager.GameplayBus.RemoveListener<AsteroidDestroyed>(OnAsteroidDestroyed);
        }

        #endregion

        #region Event callbacks

        private void OnAsteroidDestroyed(AsteroidDestroyed asteroidDestroyed)
        {
            _currentAsteroids.Remove(asteroidDestroyed.AsteroidScript);

            if (asteroidDestroyed.AsteroidData.FragmentTypesGeneratedOnDestruction.Count > 0)
                CreateAsteroidsFromDestroyedOne(asteroidDestroyed.AsteroidData.FragmentTypesGeneratedOnDestruction, asteroidDestroyed.AsteroidData.FragmentsAmountGeneratedOnDestruction, asteroidDestroyed.Position, asteroidDestroyed.Direction);
            else
                CheckRoundComplete();
        }

        #endregion

        #region Creation

        public void CreateDecorationAsteroids()
        {
            CreateNewAsteroids(_stageConfiguration.DecorationAsteroidsAmount);
        }
        public async Task CreateInitialAsteroids()
        {
            await TimingUtils.WaitSeconds(_stageConfiguration.DelayBeforeFirstRound);

            CreateNewAsteroids(_stageConfiguration.InitialAsteroidsAmount);

            _debugService.Log("Progress", $"Round {_currentRound} Started", Color.green);
        }
        public async Task CreateRoundAsteroids()
        {
            await TimingUtils.WaitSeconds(_stageConfiguration.DelayBetweenRounds);

            int asteroidsToSpawn = _stageConfiguration.InitialAsteroidsAmount + ((_currentRound - 1) * _stageConfiguration.AsteroidsIncrementPerRound);

            if (asteroidsToSpawn > _stageConfiguration.MaxPosibleAsteroids)
                asteroidsToSpawn = _stageConfiguration.MaxPosibleAsteroids;

            CreateNewAsteroids(asteroidsToSpawn);

            _debugService.Log("Progress", $"Round {_currentRound} Started", Color.green);
        }

        private void CreateNewAsteroids(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject newAsteroid = _asteroidSpawner.CreateInitialAsteroid();
                _currentAsteroids.Add(newAsteroid.GetComponent<AsteroidController>());
            }
        }
        private void CreateAsteroidsFromDestroyedOne(List<SO_AsteroidData> posibleData, int amount, Vector3 position, Vector3 originalOneDirection)
        {
            for (int i = 0; i < amount; i++)
            {
                SO_AsteroidData dataToUse = posibleData[Random.Range(0, posibleData.Count)];
                GameObject newAsteroid = _asteroidSpawner.CreateAsteroidFromDestroyedOne(dataToUse, position, originalOneDirection);
                _currentAsteroids.Add(newAsteroid.GetComponent<AsteroidController>());
            }
        }

        #endregion

        #region Management

        private void CheckRoundComplete()
        {
            if (_currentAsteroids.Count > 0)
                return;

            _debugService.Log("Progress", $"Round {_currentRound} Complete", Color.green);
            _currentRound++;

            _ = CreateRoundAsteroids();
        }

        public void RestartRound()
        {
            _currentRound = 1;
        }

        public void CleanCurrentAsteroids()
        {
            foreach (AsteroidController item in _currentAsteroids)
                _ = item.CleanAsteroid();

            _currentAsteroids.Clear();
        }

        #endregion
    }
}