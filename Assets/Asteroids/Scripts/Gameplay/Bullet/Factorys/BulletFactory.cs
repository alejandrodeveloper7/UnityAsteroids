using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Pooling.Core;
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