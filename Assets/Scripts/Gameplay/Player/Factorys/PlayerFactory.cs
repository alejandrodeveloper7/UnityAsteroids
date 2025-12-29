using Asteroids.Core.ScriptableObjects.Data;
using ToolsACG.Pooling.Core;
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