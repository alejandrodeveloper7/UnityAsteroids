using UnityEngine;

namespace ToolsACG.Utils.Pooling
{
    [CreateAssetMenu(fileName = "FactorySettings", menuName = "ScriptableObjects/ToolsACG/Pooling/FactorySettings")]
    public class SO_FactorySettings : ScriptableObject
    {
        [Header("2D AudioSources Pool Configuration")]
        public int AudiSourcesPoolInitialSize = 10;
        public int AudiSourcesPoolEscalation = 2;
        public int AudiSourcesPoolMaxSize = 150;

        [Header("GameObject Pools Parent Configuration")]
        public string ParentName = "pooled_gameObjects_parent";
        public Vector3 ParentPosition = new Vector3(0, 50, 0);

        [Header("Persistent Pools Data")]
        public SO_PersistentPoolData PersistentPoolData;
    }
}
