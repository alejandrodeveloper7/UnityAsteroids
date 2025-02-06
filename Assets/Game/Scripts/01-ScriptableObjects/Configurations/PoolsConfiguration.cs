using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfiguration", menuName = "ScriptableObjects/Configurations/PoolConfiguration")]
public class PoolsConfiguration : ScriptableObject
{
    [Header("GeneralConfiguration")]
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
    public int Escalation;
    public int InitialSize;
    public int MaxSize;
}
