using ACG.Tools.Runtime.Pooling.Core;
using Asteroids.Core.ScriptableObjects.Data;
using UnityEngine;

namespace Asteroids.Gameplay.Asteroids.Factorys
{
    public class AsteroidFactory
    {   
        #region Creation

        public GameObject GetInstance(SO_AsteroidData data)
        {
            GameObject newInstance = FactoryManager.Instance.GetGameObjectInstance(data.PrefabData);            
            return newInstance;
        }

        #endregion
    }
}
