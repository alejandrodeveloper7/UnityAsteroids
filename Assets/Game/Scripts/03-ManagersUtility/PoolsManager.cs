using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PoolsManager
{
    #region Fields
    
    public static PoolsManager Instance { get; } = new PoolsManager();

    private static PoolsConfiguration _poolsConfiguration;
    private static Dictionary<string, SimplePool> _pools;
    [Space]
    private static Transform _parentTransform;

    #endregion

    #region Initialization

    private PoolsManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        _poolsConfiguration = ResourcesManager.Instance.PoolsConfiguration;
        CreatePoolGameObjectsParent();
        CreatePools();
    }

    #endregion

    #region Pools Creation

    private void CreatePoolGameObjectsParent()
    {
        _parentTransform = new GameObject(_poolsConfiguration.ParentName).transform;
        _parentTransform.position = _poolsConfiguration.ParentPosition;
    }

    private void CreatePools()
    {
        _pools = new Dictionary<string, SimplePool>();

        foreach (PoolData poolConfiguration in _poolsConfiguration.PoolsData)
            CreatePool(poolConfiguration);
    }

    private void CreatePool(PoolData pConfiguration)
    {
        SimplePool newPool = new SimplePool(pConfiguration.Prefab, _parentTransform, pConfiguration.InitialSize, pConfiguration.Escalation, pConfiguration.MaxSize);
        _pools.Add(pConfiguration.Name, newPool);
    }

    #endregion

    #region Pools Management

    public GameObject GetInstance(string pPoolName)
    {
        _pools.TryGetValue(pPoolName, out SimplePool pool);
        
        if (pool is null) 
        {
            Debug.LogError(string.Format("Pool with the name {0} not found", pPoolName));
            return null;
        }

        return pool.GetInstance();    
    }

    #endregion
}
