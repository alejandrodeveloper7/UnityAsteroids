using System;
using UnityEngine;

namespace ToolsACG.Pooling.Models
{
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
}