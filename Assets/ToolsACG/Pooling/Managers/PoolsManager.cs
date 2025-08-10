using System.Collections.Generic;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private Transform _pooledGameObjectsParentTransform;

    [Header("Pools")]
    private List<SimpleGameObjectPool> _gameObjectsPools = new List<SimpleGameObjectPool>();

    [Header("Data")]
    [SerializeField] private SO_GameObjectPoolData _gameObjectPoolsData;

    #endregion

    #region Initialization

    private void Initialize()
    {
        if (_gameObjectPoolsData == null)
        {
            Debug.LogError(string.Format("- POOL - SO_GameObjectPoolData in {0} is null", gameObject.name));
            return;
        }

        CreatePoolGameObjectsParent();
        CreateGameObjectPools();
    }

    private void CleanManager()
    {
        DestroyGameObjectPools();
        DestroyPoolGameObjectsParent();
    }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        CleanManager();
    }

    #endregion

    #region Parent Management

    private void CreatePoolGameObjectsParent()
    {
        GameObject newGameObject = new GameObject(_gameObjectPoolsData.ParentName);
        _pooledGameObjectsParentTransform = newGameObject.transform;
        _pooledGameObjectsParentTransform.position = _gameObjectPoolsData.ParentPosition;
    }

    private void DestroyPoolGameObjectsParent()
    {
        Destroy(_pooledGameObjectsParentTransform?.gameObject);
    }

    #endregion

    #region Pools Management

    private void DestroyGameObjectPools()
    {
        foreach (var pool in _gameObjectsPools)
            pool.DestroyPool();

        _gameObjectsPools.Clear();
      
        Debug.Log(string.Format("- POOL - {0} gameObject pools destroyed", gameObject.name));
    }

    private void CreateGameObjectPools()
    {
        foreach (PoolData data in _gameObjectPoolsData.PoolsData)
            CreateGameObjectPool("", data);

        Debug.Log(string.Format("- POOL - {0} gameObject pools created", gameObject.name));
    }

    private void CreateGameObjectPool(string pSceneName, PoolData pConfiguration)
    {
        SimpleGameObjectPool newPool = new SimpleGameObjectPool(pConfiguration.PoolName, pSceneName, pConfiguration.Prefab, _pooledGameObjectsParentTransform, pConfiguration.InitialSize, pConfiguration.Escalation, pConfiguration.MaxSize);
        _gameObjectsPools.Add(newPool);
    }

    #endregion

    #region Instances Management

    public GameObject GetGameObjectInstance(string pPoolName)
    {
        foreach (SimpleGameObjectPool pool in _gameObjectsPools)
            if (pool.PoolName == pPoolName)
                return pool.GetInstance();

        Debug.LogError(string.Format("- POOL - Pool with the name {0} not found", pPoolName));
        return null;
    }

    #endregion
}
