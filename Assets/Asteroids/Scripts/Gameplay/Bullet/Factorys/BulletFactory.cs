using ACG.Tools.Runtime.Pooling.Core;
using Asteroids.Core.ScriptableObjects.Data;
using UnityEngine;

namespace Asteroids.Gameplay.Bullets.Factorys
{
    public class BulletFactory
    {
        #region Creation

        public GameObject GetInstance(SO_BulletData data)
        {
            GameObject newInstance = FactoryManager.Instance.GetGameObjectInstance(data.PrefabData);
            return newInstance;
        }

        #endregion
    }
}