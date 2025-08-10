using ToolsACG.Utils.Pooling;
using UnityEngine;

public class SimplePooledGameObjectController : MonoBehaviour, IPooleableGameObject
{
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    //This script is for pool items that dont need any controller or specific code

    //ThisGameObjectReference.GetComponent<IPooleableGameObject>().Recycle(ThisGameObjectReference);
}