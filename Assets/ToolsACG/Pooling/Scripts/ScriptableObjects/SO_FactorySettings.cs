using System.Collections.Generic;
using UnityEngine;

namespace ToolsACG.Pooling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FactorySettings", menuName = "ToolsACG/Pooling/FactorySettings")]
    public class SO_FactorySettings : ScriptableObject
    {
        #region Singleton

        private static SO_FactorySettings _instance;
        public static SO_FactorySettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_FactorySettings>("Settings/FactorySettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_FactorySettings)} SO found in Resources");
                }
                return _instance;
            }
        }

        #endregion

        #region Values

        [Header("AudioSources Pool Configuration")]

        [Space]

        [SerializeField] private bool _useAudioSourcePool;
        public bool UseAudioSourcePool => _useAudioSourcePool;

        [Space]

        [SerializeField] private string _audioSourcePoolParentName = "Pooled_AudioSources_parent";
        public string AudioSourcePoolParentName => _audioSourcePoolParentName;

        [SerializeField] private Vector3 _audioSourcePoolParentPosition = new(0, 50, 0);
        public Vector3 AudioSourcePoolParentPosition => _audioSourcePoolParentPosition;

        [SerializeField] private int _audiSourcesPoolInitialSize = 10;
        public int AudiSourcesPoolInitialSize => _audiSourcesPoolInitialSize;

        [SerializeField] private int _audiSourcesPoolEscalation = 2;
        public int AudiSourcesPoolEscalation => _audiSourcesPoolEscalation;

        [SerializeField] private int _audiSourcesPoolMaxSize = 150;
        public int AudiSourcesPoolMaxSize => _audiSourcesPoolMaxSize;

        [Space(20)]

        [Header("3D Sounds Pool Configuration")]

        [Space]

        [SerializeField] private bool _use3DSoundPool;
        public bool Use3DSoundPool => _use3DSoundPool;

        [Space]

        [SerializeField] private string _3DSoundPoolParentName = "Pooled_3DSounds_parent";
        public string Sound3DPoolParentName => _3DSoundPoolParentName;

        [SerializeField] private Vector3 _3DSoundPoolParentPosition = new(0, 50, 0);
        public Vector3 Sound3DPoolParentPosition => _3DSoundPoolParentPosition;

        [SerializeField] private GameObject _3DSoundPrefab;
        public GameObject Sound3DPrefab => _3DSoundPrefab;

        [SerializeField] private int _3DSoundPoolInitialSize = 10;
        public int Sound3DPoolInitialSize => _3DSoundPoolInitialSize;

        [SerializeField] private int _3DSoundPoolEscalation = 2;
        public int Sound3DPoolEscalation => _3DSoundPoolEscalation;

        [SerializeField] private int _3DSoundPoolMaxSize = 150;
        public int Sound3DPoolMaxSize => _3DSoundPoolMaxSize;

        [Space(20)]

        [Header("Persistent Pools Configuration")]

        [Space]

        [SerializeField] private bool _usePersistentPools;
        public bool UsePersistentPools => _usePersistentPools;

        [Space]

        [SerializeField] private string _persistentPoolsParentName = "Persistent_pooled_gameObjects_parent";
        public string PersistentPoolsParentName => _persistentPoolsParentName;

        [SerializeField] private Vector3 _persistentPoolsParentPosition = new(0, 50, 0);
        public Vector3 PersistentPoolsParentPosition => _persistentPoolsParentPosition;

        [SerializeField] private SO_PersistentPoolData _persistentPoolsData;
        public SO_PersistentPoolData PersistentPoolsData => _persistentPoolsData;

        [Space(20)]

        [Header("Scene Pool Data")]

        [Space]

        [SerializeField] private bool _useScenePools;
        public bool UseScenePools => _useScenePools;

        [Space]

        [SerializeField] private string _scenePoolsParentName = "Scene_pooled_gameObjects_parent";
        public string ScenePoolsParentName => _scenePoolsParentName;

        [SerializeField] private Vector3 _scenePoolsParentPosition = new(0, 50, 0);
        public Vector3 ScenePoolsParentPosition => _scenePoolsParentPosition;

        [SerializeField] private List<SO_ScenePoolData> _scenesPoolData;
        public List<SO_ScenePoolData> ScenesPoolData => _scenesPoolData;

        #endregion
    }
}
