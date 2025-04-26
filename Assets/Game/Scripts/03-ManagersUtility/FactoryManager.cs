using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class FactoryManager
{
    #region Fields

    public static FactoryManager Instance { get; } = new FactoryManager();

    private FactorySettings _factorySettings;
    [Space]
    private Dictionary<string, SimpleGameObjectPool> _pools;
    private AudioSourcePool _2DAudioSourcesPool;
    [Space]
    private Transform _parentTransform;

    #endregion

    #region Initialization

    private FactoryManager()
    {
        GetReferences();
        Initialize();
    }

    private void GetReferences() 
    {
        _factorySettings = ResourcesManager.Instance.GetScriptableObject<FactorySettings>(ScriptableObjectKeys.FACTORY_SETTINGS_KEY);
    }

    private void Initialize()
    {
        Create2DAudioSourcePool();

        CreatePoolGameObjectsParent();
        CreatePools();
    }

    #endregion

    #region Pools Creation

    private void Create2DAudioSourcePool()
    {
        _2DAudioSourcesPool= new AudioSourcePool(_factorySettings.AudiSourcesPoolInitialSize, _factorySettings.AudiSourcesPoolEscalation, _factorySettings.AudiSourcesPoolMaxSize);
    }


    private void CreatePoolGameObjectsParent()
    {
        _parentTransform = new GameObject(_factorySettings.ParentName).transform;
        _parentTransform.position = _factorySettings.ParentPosition;
    }

    private void CreatePools()
    {
        _pools = new Dictionary<string, SimpleGameObjectPool>();

        foreach (PoolData poolConfiguration in _factorySettings.PoolsData)
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
