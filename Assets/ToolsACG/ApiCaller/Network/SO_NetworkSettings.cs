using UnityEngine;

namespace ToolsACG.Networking
{
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "ScriptableObjects/ToolsACG/ApiCallerCreator/NetworkSettings")]
    public class SO_NetworkSettings : ScriptableObject
    {

        [Header("Environment")]
        [SerializeField] private NetworkManager.Environment _environment;
        public NetworkManager.Environment Environment { get { return _environment; } set { _environment = value; } }

        [Header("API Urls")]
        [SerializeField] private string _developmentUrl;
        [SerializeField] private string _integrationUrl;
        [SerializeField] private string _productionUrl;
        [SerializeField] private string _localhostUrl;
        public string Url
        {
            get
            {
                switch (_environment)
                {
                    case NetworkManager.Environment.Integration:
                        return _integrationUrl;
                    case NetworkManager.Environment.Development:
                        return _developmentUrl;
                    case NetworkManager.Environment.Production:
                        return _productionUrl;
                    case NetworkManager.Environment.LocalHost:
                        return _localhostUrl;
                }
                return _developmentUrl;
            }
        }

        [Header("Keys")]
        [SerializeField] private string _privateKey;
        public string PrivateKey { get { return _privateKey; } }
        [SerializeField] private string _publicKey;
        public string PublicKey { get { return _publicKey; } }

        [Header("Time out")]
        [SerializeField] private int _timeOutMs;
        public int TimeOutMs { get { return _timeOutMs; } set { _timeOutMs = value; } }

        [Header("Log")]
        public bool LogApiCall = true;
        public bool LogRequest = true;
        public bool LogResponse = true;
        public bool LogResponseTime = true;


        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------


        #region Settings and Configurations ScriptableObjects

        //References to ScriptableObjects that need be configured in specific environment, platform, store...

        #endregion

        #region Environment Configuration Methods

        [ContextMenu("Configure Environment/Development")]
        public void ConfigureDevelopmentBuild()
        {
            _environment = NetworkManager.Environment.Development;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/Integration")]
        public void ConfigureIntegrationBuild()
        {
            _environment = NetworkManager.Environment.Integration;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/Production")]
        public void ConfigureProductionBuild()
        {
            _environment = NetworkManager.Environment.Production;
            //Apply changes for this environment
        }

        [ContextMenu("Configure Environment/LocalHost")]
        public void ConfigureLocalHostBuild()
        {
            _environment = NetworkManager.Environment.LocalHost;
            //Apply changes for this environment
        }

        #endregion

        #region Version Configuration Methods

        [ContextMenu("Configure Version/Steam Windows")]
        public void ConfigureSteamWindowsBuild()
        {
            //Apply changes for this Version
        }

        [ContextMenu("Configure Version/Steam Mac")]
        public void ConfigureSteamMacBuild()
        {
            //Apply changes for this Version
        }

        [ContextMenu("Configure Version/Epic Windows")]
        public void ConfigureEpicWindowsBuild()
        {
            //Apply changes for this Version
        }

        [ContextMenu("Configure Version/Epic Mac")]
        public void ConfigureEpicMacBuild()
        {
            //Apply changes for this Version
        }

        [ContextMenu("Configure Version/Android Google Play")]
        public void ConfigureAndroidGooglePlayBuild()
        {
            //Apply changes for this Version
        }

        [ContextMenu("Configure Version/iOS App Store")]
        public void ConfigureiOSAppStoreBuild()
        {
            //Apply changes for this Version
        }

        #endregion               
    }
}
