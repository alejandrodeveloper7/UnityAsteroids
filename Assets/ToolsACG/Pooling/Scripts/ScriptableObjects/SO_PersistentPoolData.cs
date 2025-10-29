using System.Collections.Generic;
using ToolsACG.Pooling.Models;
using UnityEngine;

namespace ToolsACG.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PersistentPoolData", menuName = "ToolsACG/Pooling/PersistentPoolData")]
    public class SO_PersistentPoolData : ScriptableObject
    {
        #region Values

        [Header("References")]

        [SerializeField] private List<PoolData> _poolsData;
        public List<PoolData> PoolsData => _poolsData;

        #endregion
    }
}
