using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.Player.Controllers;
using Asteroids.Gameplay.Player.Factorys;
using ToolsACG.Core.Services;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Spawners
{
    public class PlayerSpawner
    {
        #region Fields

        [Header("References")]
        private readonly PlayerFactory _factory;
        [Space]
        private readonly IContainerRuntimeDataService _runtimeDataService;

        #endregion

        #region Constructors

        [Inject]
        public PlayerSpawner(PlayerFactory factory, IContainerRuntimeDataService runtimeDataService)
        {
            _factory = factory;
            _runtimeDataService = runtimeDataService;
        }

        #endregion

        #region Spawn

        public GameObject SpawnSelectedPlayer()
        {
            SO_ShipData shipData = _runtimeDataService.Data.SelectedShipData;
            return SpawnSpecificPlayer(shipData);
        }

        public GameObject SpawnSpecificPlayer(SO_ShipData data)
        {
            GameObject newInstance = _factory.GetInstance(data);
            newInstance.transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);

            PlayerController controller = newInstance.GetComponent<PlayerController>();
            controller.Initialize(data);

            return newInstance;
        }

        #endregion
    }
}