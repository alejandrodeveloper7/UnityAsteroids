using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentPoolData", menuName = "ScriptableObjects/ToolsACG/Pooling/PersistentPoolData")]
public class SO_PersistentPoolData : ScriptableObject
{
    [Header("Pools data")]
    public List<PoolData> PoolsData;
}
