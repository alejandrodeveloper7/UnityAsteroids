using System;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;

namespace ACG.Tools.Runtime.ApiCallersCreator.Models
{
    [Serializable]
    public class NetworkConfigurationSetup
    {
        public NetworkConfigurationType NetworkConfigurationType;
        public SO_NetworkConfiguration NetworkConfiguration;
    }
}