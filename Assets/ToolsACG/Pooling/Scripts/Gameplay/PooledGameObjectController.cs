using ToolsACG.Pooling.Interfaces;
using ToolsACG.Pooling.Pools;
using UnityEngine;

namespace ToolsACG.Pooling.Gameplay
{
    public class PooledGameObjectController : MonoBehaviour, IPooleableGameObject
    {
        #region Fields

        //[Header("IPooleableGameObject")]
        public GameObjectPool OriginPool { get; set; }
        public bool ReadyToUse { get; set; }

        #endregion

        #region Functionality

        public void RecycleGameObject()
        {
            GetComponent<IPooleableGameObject>().Recycle(gameObject);
        }

        #endregion
    }
}