using System.Collections.Generic;
using ToolsACG.Pooling.Models;
using UnityEngine;

namespace ToolsACG.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewScenePoolData", menuName = "ToolsACG/Pooling/ScenePoolData")]
    public class SO_ScenePoolData : ScriptableObject
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

        [SerializeField] private List<PoolData> _poolsData;
        public List<PoolData> PoolsData => _poolsData;

        #endregion
    }
}