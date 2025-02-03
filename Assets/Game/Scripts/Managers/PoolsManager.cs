using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;
using static PoolSettings;

public class PoolsManager
{
    public static PoolsManager Instance { get; } = new PoolsManager();

    private static PoolSettings _poolSettings;
    private static Dictionary<string, SimplePool> _pools;
    [Space]
    private static Transform _parentTransform;
      

    private PoolsManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        _poolSettings = Resources.Load<PoolSettings>("Settings/PoolSettings");
        CreatePoolGameObjectsParent();
        CreatePools();
    }

    #region Pools Creation

    private void CreatePoolGameObjectsParent()
    {
        _parentTransform = new GameObject(_poolSettings.ParentName).transform;
        _parentTransform.position = _poolSettings.ParentPosition;
    }

    private void CreatePools()
    {
        _pools = new Dictionary<string, SimplePool>();

        foreach (PoolConfiguration poolConfiguration in _poolSettings.PoolConfigurations)
            CreatePool(poolConfiguration);
    }

    private void CreatePool(PoolConfiguration pConfiguration)
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
