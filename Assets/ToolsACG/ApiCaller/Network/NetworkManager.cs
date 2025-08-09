using System;
using UnityEngine;

namespace ToolsACG.Networking
{
    public class NetworkManager
    {
        #region Enums

        [Serializable]
        public enum Environment
        {
            Development = 0,
            Production = 10,
            Integration = 20,
            LocalHost = 30,
        }

        #endregion

        #region Properties

        public static NetworkManager Instance { get; } = new NetworkManager();
        public SO_NetworkSettings NetworkSetting { get; private set; }

        #endregion

        #region Constructors

        static NetworkManager()
        {
            Instance.Initialize();
        }

        #endregion

        #region Initialization

        public void Initialize()
        {
            NetworkSetting = Resources.Load<SO_NetworkSettings>("NetworkSettings");
        }

        #endregion
    }
}
