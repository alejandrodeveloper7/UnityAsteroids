using ACG.Tools.Runtime.Pooling.Core;
using Asteroids.Core.ScriptableObjects.Configurations;
using UnityEngine;

namespace Asteroids.Gameplay.FloatingText.Factorys
{
    public class FloatingTextFactory
    {
        #region Creation

        public GameObject GetInstance(SO_FloatingTextConfiguration configuration)
        {
            GameObject newInstance = FactoryManager.Instance.GetGameObjectInstance(configuration.PrefabData);
            return newInstance;
        }

        #endregion
    }
}