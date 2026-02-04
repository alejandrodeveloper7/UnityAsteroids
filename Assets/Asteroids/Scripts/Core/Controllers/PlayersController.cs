using Asteroids.Core.Events.Gameplay;
using Asteroids.Gameplay.Player.Spawners;
using ToolsACG.Core.EventBus;
using ToolsACG.Core.Services;
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

            EventBusManager.GameplayBus.AddListener<PlayerDied>(OnPlayerDied);
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
