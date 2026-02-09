using ACG.Core.EventBus;
using Asteroids.Core.Events.Gameplay;
using Asteroids.Core.Services;
using Asteroids.Gameplay.Player.Spawners;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class PlayersController
    {
        #region Fields

        [Header("References")]
        private readonly PlayerSpawner _playerSpawner;
        [Space]
        private readonly IContainerRuntimeDataService _runtimeDataService;

        public GameObject CurrentPlayer { get; private set; }

        #endregion

        #region Constructors

        [Inject]
        public PlayersController(PlayerSpawner spawner, IContainerRuntimeDataService containerRuntimeDataService)
        {
            _playerSpawner = spawner;
            _runtimeDataService = containerRuntimeDataService;

            CreateListeners();
        }

        public void Dispose()
        {
            RemoveListeners();
        }

        private void CreateListeners()
        {
            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
        }

        private void RemoveListeners()
        {
            EventBusManager.GameplayBus.RemoveListener<PlayerDied>(OnPlayerDied);
        }

        #endregion

        #region Event callbacks

        private void OnPlayerDied(PlayerDied playerDied)
        {
            CurrentPlayer = null;
        }

        #endregion

        #region Players management

        public GameObject CreatePlayer()
        {
            CurrentPlayer = _playerSpawner.SpawnPlayer(_runtimeDataService.Data.SelectedShipData);
            return CurrentPlayer;
        }

        #endregion

    }
}
