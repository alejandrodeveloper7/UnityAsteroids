using System.Collections.Generic;
using ToolsACG.Pooling.Models;
using UnityEngine;

namespace ToolsACG.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPoolContainerData", menuName = "ToolsACG/Pooling/PoolContainerData")]
    public class SO_PoolContainerData : ScriptableObject
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private string _parentName = "Pooled_gameObjects_parent";
        public string ParentName => _parentName;

        [SerializeField] private List<PoolData> _poolsData;
        public List<PoolData> PoolsData => _poolsData;

        #endregion
    }
}
