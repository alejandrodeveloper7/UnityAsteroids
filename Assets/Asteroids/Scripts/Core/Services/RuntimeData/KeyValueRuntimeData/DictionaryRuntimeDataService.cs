using System;
using System.Collections.Generic;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using UnityEngine;

namespace Asteroids.Core.Services
{
    public class DictionaryRuntimeDataService : SingletonServiceBase<DictionaryRuntimeDataService>, IKeyValueRuntimeDataService
    {
        #region Fields
              
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }

        [Header("Data")]
        private readonly Dictionary<string, object> _data = new();

        #endregion

        #region Initialization     

        // Uncomment this to enable auto-instantiation on play and make it an auto singleton
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //private static void AutoInit() => EnsureInstance();

        public void Setup()
        {
            // TODO: Manual method called to set parameters
        }

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion       

        #region Functionality

        public void Set<T>(string key, T value)
        {
            _data[key] = value;
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            if (_data.TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;

            return defaultValue;
        }

        public void Clear()
        {
            _data.Clear();
        }

        #endregion
    }
}