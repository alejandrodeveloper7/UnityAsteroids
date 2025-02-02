using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;
using static PoolSettings;

public class PoolsManager : MonoBehaviour
{
    #region Fields

    private PoolSettings _poolSettings;
    private Dictionary<string, SimplePool> _pools = new Dictionary<string, SimplePool>();
    [Space]
    private Transform _parentTransform;

    #endregion

    #region Singleton

    private static PoolsManager m_instance;
    public static PoolsManager Instance => m_instance;

    private void CreateSingleton()
    {
        if (m_instance != null)
            Destroy(this);
        else
            m_instance = this;
    }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        CreateSingleton();

        _poolSettings = ResourcesManager.Instance.PoolSettings;
        CreatePoolGameObjectsParent();
        CreatePools();
    }

    #endregion

    #region Pools Creation

    private void CreatePoolGameObjectsParent()
    {
        _parentTransform = new GameObject(_poolSettings.ParentName).transform;
        _parentTransform.position = _poolSettings.ParentPosition;
    }

    private void CreatePools()
    {
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
