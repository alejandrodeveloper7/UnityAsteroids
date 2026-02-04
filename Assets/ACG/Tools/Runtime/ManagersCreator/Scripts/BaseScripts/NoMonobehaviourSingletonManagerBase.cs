using ACG.Tools.Runtime.ManagersCreator.Interfaces;
using UnityEngine;

namespace ACG.Tools.Runtime.ManagersCreator.Bases
{
    public abstract class NoMonobehaviourSingletonManagerBase<T> : IManager where T : NoMonobehaviourSingletonManagerBase<T>, new()
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
        }

        #endregion

        #region Initilization

        protected static void EnsureInstance()
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Initialize();
                Debug.Log($"- {typeof(T).Name} - persistent manager initialized");
            }
        }

        public virtual void Initialize()
        {
            Application.quitting += OnApplicationQuit;
        }

        public virtual void Dispose()
        {
            Application.quitting -= OnApplicationQuit;
        }

        #endregion

        #region Event Callbacks

        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
            Dispose();
        }

        #endregion 
    }
}