using ToolsACG.Pooling.Pools;
using UnityEngine;

namespace ToolsACG.Pooling.Interfaces
{
    public interface IPooleableGameObject
    {
        GameObjectPool OriginPool { get; set; }
        bool ReadyToUse { get; set; }

        public void Recycle(GameObject instance)
        {
            if (OriginPool is null)
            {
                Debug.Log($"OriginPool is null, gameObject destroyed - {instance.name}");
                GameObject.Destroy(instance);
            }
            else
                OriginPool.RecycleGameObject(instance);
        }
    }
}