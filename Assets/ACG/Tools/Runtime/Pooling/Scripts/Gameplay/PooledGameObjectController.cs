using ACG.Tools.Runtime.Pooling.Interfaces;
using ACG.Tools.Runtime.Pooling.Pools;
using UnityEngine;

namespace ACG.Tools.Runtime.Pooling.Gameplay
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