using ToolsACG.Utils.Pooling;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour, IPooleableGameObject
{
    #region Fields

    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private ParticleSystem _particleSystem;
    
    #endregion

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void RecycleGameObject()
    {
        OriginPool.RecycleItem(gameObject);
    }
    
    public void Play()
    {
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        _particleSystem.Play();
        Invoke(nameof(RecycleGameObject), mainModule.duration);
    }

}
