using System.Collections.Generic;
using System.Linq;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using UnityEngine;

namespace ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "ToolsACG/ApiCallerCreator/NetworkSettings")]
    public class SO_NetworkSettings : ScriptableObject
    {
        #region Singleton

        private static SO_NetworkSettings _instance;
        public static SO_NetworkSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SO_NetworkSettings>("Settings/NetworkSettings");
                    if (_instance == null)
                        Debug.LogError($"No {nameof(SO_NetworkSettings)} found in Resources");
                }
                return _instance;
            }
        }

        #endregion
       
        #region Values

        [Header("Environment")]

        [SerializeField] private ServerEnvironment _environment;
        public ServerEnvironment Environment { get { return _environment; } set { _environment = value; } }


        [Header("Network Configurations")]
        [SerializeField] private List<NetworkConfigurationSetup> _networkConfigurations;
        public SO_NetworkConfiguration GetNetworkConfiguration(NetworkConfigurationType type)
        {
            return _networkConfigurations.FirstOrDefault(x => x.NetworkConfigurationType == type).NetworkConfiguration;
        }


        [Header("Log")]

        [SerializeField] private bool _logApiCall = true;
        public bool LogApiCall => _logApiCall;

        [SerializeField] private bool _logRequest = true;
        public bool LogRequest => _logRequest;

        [SerializeField] private bool _logResponse = true;
        public bool LogResponse => _logResponse;

        [SerializeField] private bool _logResponseTime = true;
        public bool LogResponseTime => _logResponseTime;

        #endregion

        #region Settings and Configurations References

        //References to ScriptableObjects that need specific configuration in each environment

        #endregion

        #region Environment Configuration Methods

        [ContextMenu("Configure Environment/Development")]
        public void ConfigureDevelopmentBuild()
        {
            _environment = ServerEnvironment.Development;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/Integration")]
        public void ConfigureIntegrationBuild()
        {
            _environment = ServerEnvironment.Integration;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/Production")]
        public void ConfigureProductionBuild()
        {
            _environment = ServerEnvironment.Production;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/LocalHost")]
        public void ConfigureLocalHostBuild()
        {
            _environment = ServerEnvironment.LocalHost;
            //Apply changes for this environment
        }

        #endregion
    }   
}
