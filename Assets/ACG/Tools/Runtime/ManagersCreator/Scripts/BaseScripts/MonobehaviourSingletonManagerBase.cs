using ACG.Tools.Runtime.ManagersCreator.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.ManagersCreator.Bases
{
    public abstract class MonobehaviourSingletonManagerBase<T> : MonoBehaviour, IManager, IDisposable, IInitializable where T : MonobehaviourSingletonManagerBase<T>
    {
        #region Fields

        [Header("Singleton")]

        private static bool _applicationIsQuitting;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                    return null;

                EnsureInstance();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        #endregion

        #region Initialization     

        protected static void EnsureInstance()
        {
            if (_instance == null)
            {
                GameObject prefab = Resources.Load<GameObject>($"Managers/{typeof(T).Name}");

                if (prefab == null)
                {
                    Debug.LogError($"- {typeof(T).Name} - No prefab found in Resources/Managers/");
                    return;
                }

                GameObject instancedPrefab = Instantiate(prefab);
                DontDestroyOnLoad(instancedPrefab);
                _instance = instancedPrefab.GetComponent<T>();
                Debug.Log($"- {typeof(T).Name} - persistent prefab auto instanced");
            }
        }

        protected virtual void GetReferences() { }
        public virtual void Initialize() { }
        public virtual void Dispose() { }

        #endregion

        #region Monobehaviour

        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = (T)this;
            else
                Destroy(gameObject);
        }

        protected virtual void Start() 
        {
        
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        #endregion
    }
}
