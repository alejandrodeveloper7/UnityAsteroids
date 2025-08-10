using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectPoolData", menuName = "ScriptableObjects/ToolsACG/Pooling/GameObjectPoolData")]
public class SO_GameObjectPoolData : ScriptableObject
{
    [Header("Pools Parent Configuration")]
    public string ParentName = "_pooled_gameObjects_parent";
    public Vector3 ParentPosition = new Vector3(0, 50, 0);

    [Header("Pools data")]
    public List<PoolData> PoolsData;
}
