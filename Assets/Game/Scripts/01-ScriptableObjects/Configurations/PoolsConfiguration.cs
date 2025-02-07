using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfiguration", menuName = "ScriptableObjects/Configurations/PoolConfiguration")]
public class PoolsConfiguration : ScriptableObject
{
    [Header("2D AudioSources Configuration")]
    public int AudiSourcesPoolInitialSize;
    public int AudiSourcesPoolEscalation;
    public int AudiSourcesPoolMaxSize;

    [Header("General Configuration")]
    public string ParentName;
    public Vector3 ParentPosition;

    [Header("Pools data")]
    public List<PoolData> PoolsData;        
}

[Serializable]
public class PoolData
{
    public string Name;
    public GameObject Prefab;
    [Space]
    public int InitialSize;
    public int Escalation;
    public int MaxSize;
}
