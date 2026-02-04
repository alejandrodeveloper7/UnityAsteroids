using System.Collections.Generic;
using UnityEngine;

namespace ACG.Tools.Runtime.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewScenePoolData", menuName = "ToolsACG/Pooling/ScenePoolData")]
    public class SO_ScenePoolData : ScriptableObject
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

        [SerializeField] private List<SO_PooledGameObjectData> _gameObjectesData;
        public List<SO_PooledGameObjectData> GameObjectesData => _gameObjectesData;

        #endregion
    }
}