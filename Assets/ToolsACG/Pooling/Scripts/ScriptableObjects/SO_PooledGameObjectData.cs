using UnityEngine;

namespace ToolsACG.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPooledGameObjectData", menuName = "ToolsACG/Pooling/PooledGameObjectData")]
    public class SO_PooledGameObjectData : ScriptableObject
    {
        #region Values

        [Header("Configuration")]

        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [Space]

        [SerializeField] private int _initialSize;
        public int InitialSize => _initialSize;

        [SerializeField] private int _scalation;
        public int Scalation => _scalation;

        [SerializeField] private int _maxSize;
        public int MaxSize => _maxSize;

        #endregion
    }
}