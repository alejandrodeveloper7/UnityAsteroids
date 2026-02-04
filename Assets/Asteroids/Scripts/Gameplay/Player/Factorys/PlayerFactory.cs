using ACG.Tools.Runtime.Pooling.Core;
using Asteroids.Core.ScriptableObjects.Data;
using UnityEngine;

namespace Asteroids.Gameplay.Player.Factorys
{
    public class PlayerFactory
    {
        #region Creation

        public GameObject GetInstance(SO_ShipData data)
        {
            GameObject newInstance = FactoryManager.Instance.GetGameObjectInstance(data.PrefabData);            
            return newInstance;
        }

        #endregion
    }
}