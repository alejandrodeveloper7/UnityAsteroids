using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Gameplay.FloatingText.Controllers;
using Asteroids.Gameplay.FloatingText.Factorys;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.FloatingText.Spawners
{
    public class FloatingTextSpawner
    {
        #region Fields

        [Header("References")]
        private readonly FloatingTextFactory _factory;
        private readonly Transform _floatingTextParent;

        [Header("Data")]
        private readonly SO_FloatingTextConfiguration _configuration;

        #endregion

        #region Constructors

        [Inject]
        public FloatingTextSpawner(SO_FloatingTextConfiguration configuration, FloatingTextFactory factory)
        {
            _configuration = configuration;
            _factory = factory;

            _floatingTextParent = new GameObject("FloatingTexts").transform;
        }

        #endregion

        #region Spawn

        public GameObject Spawn(Vector3 position, string text)
        {
            GameObject newInstance = _factory.GetInstance(_configuration);
            newInstance.transform.SetParent(_floatingTextParent, false);
            newInstance.transform.SetPositionAndRotation(position, Quaternion.identity);

            FloatingTextController controller = newInstance.GetComponent<FloatingTextController>();
            controller.Initialize(text);

            return newInstance;
        }

        #endregion
    }
}