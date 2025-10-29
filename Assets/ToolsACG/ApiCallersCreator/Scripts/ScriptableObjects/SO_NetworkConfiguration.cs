using System.Collections.Generic;
using ToolsACG.ApiCallersCreator.Enums;
using ToolsACG.ApiCallersCreator.Models;
using UnityEngine;

namespace ToolsACG.ApiCallersCreator.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewNetworkConfiguration", menuName = "ToolsACG/ApiCallerCreator/NetworkConfiguration")]
    public class SO_NetworkConfiguration : ScriptableObject
    {
        #region Values

        [Header("Development")]
        [SerializeField] private string _developmentUrl;
        [SerializeField] private string _developmentPrivateKey;
        [SerializeField] private string _developmentPublicKey;
        [SerializeField] private string _developmentApiKey;


        [Header("Production")]
        [SerializeField] private string _productionUrl;
        [SerializeField] private string _productionPrivateKey;
        [SerializeField] private string _productionPublicKey;
        [SerializeField] private string _productionApiKey;


        [Header("Integration")]
        [SerializeField] private string _integrationUrl;
        [SerializeField] private string _integrationPrivateKey;
        [SerializeField] private string _integrationPublicKey;
        [SerializeField] private string _integrationApiKey;


        [Header("Local Host")]
        [SerializeField] private string _localhostUrl;
        [SerializeField] private string _localHostPrivateKey;
        [SerializeField] private string _localHostPublicKey;
        [SerializeField] private string _localHostApiKey;


        public string Url
        {
            get
            {
                return SO_NetworkSettings.Instance.Environment switch
                {
                    ServerEnvironment.Development => _developmentUrl,
                    ServerEnvironment.Production => _productionUrl,
                    ServerEnvironment.Integration => _integrationUrl,
                    ServerEnvironment.LocalHost => _localhostUrl,
                    _ => _developmentUrl,
                };
            }
        }

        public string PrivateKey
        {
            get
            {
                return SO_NetworkSettings.Instance.Environment switch
                {
                    ServerEnvironment.Development => _developmentPrivateKey,
                    ServerEnvironment.Production => _productionPrivateKey,
                    ServerEnvironment.Integration => _integrationPrivateKey,
                    ServerEnvironment.LocalHost => _localHostPrivateKey,
                    _ => _developmentPrivateKey,
                };
            }
        }

        public string PublicKey
        {
            get
            {
                return SO_NetworkSettings.Instance.Environment switch
                {
                    ServerEnvironment.Development => _developmentPublicKey,
                    ServerEnvironment.Production => _productionPublicKey,
                    ServerEnvironment.Integration => _integrationPublicKey,
                    ServerEnvironment.LocalHost => _localHostPublicKey,
                    _ => _developmentPublicKey,
                };
            }
        }

        public string ApiKey
        {
            get
            {
                return SO_NetworkSettings.Instance.Environment switch
                {
                    ServerEnvironment.Development => _developmentApiKey,
                    ServerEnvironment.Production => _productionApiKey,
                    ServerEnvironment.Integration => _integrationApiKey,
                    ServerEnvironment.LocalHost => _localHostApiKey,
                    _ => _developmentApiKey,
                };
            }
        }


        [Header("Authentication")]
        [SerializeField] private bool _requiresAuthToken;
        public bool RequiresAuthToken => _requiresAuthToken;


        [Header("Headers")]
        [SerializeField] private List<HeaderEntry> _customHeaders;
        public List<HeaderEntry> CustomHeaders => _customHeaders;


        [Header("Time out")]
        [SerializeField] private int _timeOutMs = 8000;
        public int TimeOutMs { get { return _timeOutMs; } set { _timeOutMs = value; } }

        #endregion

    }
}
