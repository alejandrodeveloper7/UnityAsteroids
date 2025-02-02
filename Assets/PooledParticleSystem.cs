using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour, IPooleableItem
{
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }


    private ParticleSystem _particleSystem;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public async void ExecuteBehaviour()
    {
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        _particleSystem.Play();
        Invoke(nameof(RecycleGameObject), mainModule.duration);
    }

    private void RecycleGameObject()
    {
        OriginPool.RecycleItem(gameObject);
    }
}
