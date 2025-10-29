using Asteroids.Core.ScriptableObjects.Configurations;
using ToolsACG.Pooling.Core;
using UnityEngine;

namespace Asteroids.Gameplay.FloatingText.Factorys
{
    public class FloatingTextFactory
    {
        #region Creation

        public GameObject GetInstance(SO_FloatingTextConfiguration configuration)
        {
            GameObject newInstance = FactoryManager.Instance.GetGameObjectInstance(configuration.PoolName);
            return newInstance;
        }

        #endregion
    }
}