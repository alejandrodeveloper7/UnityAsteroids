using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PoolsManager
{
    #region Fields

    public static PoolsManager Instance { get; } = new PoolsManager();

    private PoolsConfiguration _poolsConfiguration;
    private Dictionary<string, SimpleGameObjectPool> _pools;
    private AudioSourcePool _2DAudioSourcesPool;
    [Space]
    private Transform _parentTransform;

    #endregion

    #region Initialization

    private PoolsManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        _poolsConfiguration = ResourcesManager.Instance.PoolsConfiguration;

        Create2DAudioSourcePool();

        CreatePoolGameObjectsParent();
        CreatePools();
    }

    #endregion

    #region Pools Creation

    private void Create2DAudioSourcePool()
    {
        _2DAudioSourcesPool= new AudioSourcePool(_poolsConfiguration.AudiSourcesPoolInitialSize, _poolsConfiguration.AudiSourcesPoolEscalation, _poolsConfiguration.AudiSourcesPoolMaxSize);
    }


    private void CreatePoolGameObjectsParent()
    {
        _parentTransform = new GameObject(_poolsConfiguration.ParentName).transform;
        _parentTransform.position = _poolsConfiguration.ParentPosition;
    }

    private void CreatePools()
    {
        _pools = new Dictionary<string, SimpleGameObjectPool>();

        foreach (PoolData poolConfiguration in _poolsConfiguration.PoolsData)
            CreatePool(poolConfiguration);
    }

    private void CreatePool(PoolData pConfiguration)
    {
        SimpleGameObjectPool newPool = new SimpleGameObjectPool(pConfiguration.Prefab, _parentTransform, pConfiguration.InitialSize, pConfiguration.Escalation, pConfiguration.MaxSize);
        _pools.Add(pConfiguration.Name, newPool);
    }

    #endregion

    #region Pools Management

    public AudioSource Get2DAudioSource() 
    {
        return _2DAudioSourcesPool.GetInstance();
    }

    public GameObject GetGameObjectInstance(string pPoolName)
    {
        _pools.TryGetValue(pPoolName, out SimpleGameObjectPool pool);

        if (pool is null)
        {
            Debug.LogError(string.Format("Pool with the name {0} not found", pPoolName));
            return null;
        }

        return pool.GetInstance();
    }

    #endregion
}
