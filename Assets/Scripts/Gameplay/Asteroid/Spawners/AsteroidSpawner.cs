using Asteroids.Core.ScriptableObjects.Collections;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.Asteroids.Controllers;
using Asteroids.Gameplay.Asteroids.Factorys;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Spawners
{
    public class AsteroidSpawner
    {
        #region Fields

        [Header("References")]
        private readonly AsteroidFactory _factory;
        private readonly Transform _asteroidsParent;

        [Header("Data")]
        private readonly SO_AsteroidsCollection _asteroidsCollection;
        private readonly List<SO_AsteroidData> _initialAsteroids;

        #endregion

        #region Constructors

        [Inject]
        public AsteroidSpawner(AsteroidFactory factory, SO_AsteroidsCollection collection)
        {
            _factory = factory;
            _asteroidsCollection = collection;

            _asteroidsParent = new GameObject("Asteroids").transform;
            _initialAsteroids = _asteroidsCollection.Elements.Where(x => x.IsInitialAsteroid is true).ToList();
        }

        #endregion

        #region Spawn

        public GameObject CreateInitialAsteroid()
        {
            SO_AsteroidData newAsteroidData = _initialAsteroids[Random.Range(0, _initialAsteroids.Count)];
            GameObject newInstance = _factory.GetInstance(newAsteroidData);
            newInstance.transform.SetParent(_asteroidsParent, false);

            AsteroidController controller = newInstance.GetComponent<AsteroidController>();
            controller.Initialize(newAsteroidData);

            return newInstance;
        }

        public GameObject CreateAsteroidFromDestroyedOne(SO_AsteroidData data, Vector3 position, Vector3 originalOneDirection)
        {
            float newAngleDirection = Random.Range(-data.AsteroidSpawnAngle, data.AsteroidSpawnAngle);
            Vector3 newDirection = Quaternion.AngleAxis(newAngleDirection, Vector3.forward) * originalOneDirection;

            GameObject newInstance = _factory.GetInstance(data);
            newInstance.transform.SetParent(_asteroidsParent, false);

            AsteroidController controller = newInstance.GetComponent<AsteroidController>();
            controller.Initialize(data, position, newDirection);

            return newInstance;
        }

        #endregion
    }
}