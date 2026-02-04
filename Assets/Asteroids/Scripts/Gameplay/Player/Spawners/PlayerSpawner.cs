using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.Player.Controllers;
using Asteroids.Gameplay.Player.Factorys;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Player.Spawners
{
    public class PlayerSpawner
    {
        #region Fields

        [Header("References")]
        private readonly PlayerFactory _factory;

        [Header("Gameplay References")]
        private Transform _playersParent;

        #endregion

        #region Constructors

        [Inject]
        public PlayerSpawner(PlayerFactory factory)
        {
            _factory = factory;

            CreateParent();
        }

        #endregion

        #region Parrent management

        private void CreateParent() 
        {
            _playersParent = new GameObject("Players").transform;
        }

        #endregion

        #region Spawn

        public GameObject SpawnPlayer(SO_ShipData data)
        {
            GameObject newInstance = _factory.GetInstance(data);
            newInstance.transform.SetPositionAndRotation(Vector2.zero, Quaternion.identity);
            newInstance.transform.SetParent(_playersParent, false);

            PlayerController controller = newInstance.GetComponent<PlayerController>();
            controller.Initialize(data);

            return newInstance;
        }

        #endregion
    }
}