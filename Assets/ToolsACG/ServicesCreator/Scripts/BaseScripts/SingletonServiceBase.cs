using ToolsACG.ServicesCreator.Interfaces;
using UnityEngine;

namespace ToolsACG.ServicesCreator.Bases
{
    public abstract class SingletonServiceBase<T> : IService where T : SingletonServiceBase<T>, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                EnsureInstance();
                return _instance;
            }
        }

        protected SingletonServiceBase()
        {
            Initialize();
        }

        protected static void EnsureInstance()
        {
            if (_instance == null)
            {
                _instance = new ();
                Debug.Log($"- {typeof(T).Name} - singleton service created and initialize");
            }
        }

        private void OnAppQuit()
        {
            Dispose();
        }

        public virtual void Initialize() 
        {
            Application.quitting += OnAppQuit;
        }

        public virtual void Dispose() 
        {        
            Application.quitting -= OnAppQuit;
        }
    }
}