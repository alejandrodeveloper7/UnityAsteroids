using ACG.Tools.Runtime.ManagersCreator.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.ManagersCreator.Bases
{
    public class MonobehaviourInstancesManagerBase<T> : MonoBehaviour, IManager, IDisposable, IInitializable where T : MonobehaviourInstancesManagerBase<T>
    {
        #region Initialization     

        public static T CreateInstance()
        {
            GameObject prefab = Resources.Load<GameObject>($"Managers/{typeof(T).Name}");

            if (prefab == null)
            {
                Debug.LogError($"- {typeof(T).Name} - No prefab found in Resources/Managers/");
                return null;
            }

            GameObject instancedPrefab = Instantiate(prefab);
            return instancedPrefab.GetComponent<T>();
        }

        protected virtual void GetReferences() { }
        public virtual void Initialize() { }
        public virtual void Dispose() { }

        #endregion

        #region Monobehaviour

        protected virtual void Awake() { }
        protected virtual void Start() { }

        #endregion
    }
}
