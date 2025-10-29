using System;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.ScriptableObjects;

namespace ToolsACG.ApiCallersCreator.Models
{
    [Serializable]
    public class NetworkConfigurationSetup
    {
        public NetworkConfigurationType NetworkConfigurationType;
        public SO_NetworkConfiguration NetworkConfiguration;
    }
}