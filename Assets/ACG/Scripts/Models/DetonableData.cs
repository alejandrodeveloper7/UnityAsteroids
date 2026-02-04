using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using System;
using UnityEngine;

namespace ACG.Scripts.Models
{
    [Serializable]
    public class DetonableData 
    {
        public SO_PooledGameObjectData PrefabData;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
    }
}