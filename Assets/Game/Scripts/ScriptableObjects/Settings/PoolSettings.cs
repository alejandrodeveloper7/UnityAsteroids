using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSettings", menuName = "ScriptableObjects/Setting/PoolSettings", order = 1)]
public class PoolSettings : ScriptableObject
{
    public string ParentName;
    public Vector3 ParentPosition;
    [Space]
    public List<PoolConfiguration> PoolConfigurations;

    [Serializable]
    public class PoolConfiguration 
    {
        public string Name; 
        public GameObject Prefab;
        [Space]
        public int Escalation;
        public int InitialSize;
        public int MaxSize;    
    }
}
