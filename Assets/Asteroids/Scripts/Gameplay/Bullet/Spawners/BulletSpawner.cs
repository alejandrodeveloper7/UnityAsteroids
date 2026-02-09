using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Gameplay.Bullets.Controllers;
using Asteroids.Gameplay.Bullets.Factorys;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Spawners
{
    public class BulletSpawner
    {
        #region Fields

        [Header("References")]
        private readonly BulletFactory _factory;
        [Space]
        private Transform _bulletsParent;
        
        #endregion

        #region Constructors

        [Inject]
        public BulletSpawner(BulletFactory factory)
        {
            _factory = factory;
            CreateBulletParent();
        }

        #endregion

        #region Parent Management

        private void CreateBulletParent() 
        {
            _bulletsParent = new GameObject("Bullets").transform;
        }

        #endregion

        #region Spawn

        public GameObject Spawn(SO_BulletData data, Vector3 position, Quaternion rotation)
        {
            GameObject newInstante = _factory.GetInstance(data);
            newInstante.transform.SetParent(_bulletsParent, false);
            newInstante.transform.SetPositionAndRotation(position, rotation);

            BulletController controller = newInstante.GetComponent<BulletController>();
            controller.Initialize(data);
            return newInstante;
        }

        #endregion
    }
}