using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScenePoolData", menuName = "ScriptableObjects/ToolsACG/Pooling/ScenePoolData")]
public class SO_ScenePoolData : ScriptableObject
{
    public string SceneName;

    [Header("Pools data")]
    public List<PoolData> PoolsData;
}

[Serializable]
public class PoolData
{
    public string PoolName;
    public GameObject Prefab;
    [Space]
    public int InitialSize;
    public int Escalation;
    public int MaxSize;
}
