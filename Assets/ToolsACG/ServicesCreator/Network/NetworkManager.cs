using System;
using UnityEngine;

namespace ToolsACG.Networking
{
    public class NetworkManager
    {
        [Serializable]
        public enum Environment
        {
            Development = 0,
            Production = 10,
            Integration = 20,
            LocalHost = 30,
        }

        public static NetworkManager Instance { get; } = new NetworkManager();

        private NetworkSettings _networkSetting;
        public NetworkSettings NetworkSetting { get { return _networkSetting; } }

        static NetworkManager()
        {
            Instance.Initialize();
        }

        public void Initialize()
        {
            _networkSetting = Resources.Load<NetworkSettings>("NetworkSettings");
        }
    }
}
