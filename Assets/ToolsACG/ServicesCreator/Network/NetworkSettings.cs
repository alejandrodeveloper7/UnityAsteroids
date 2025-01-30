using UnityEngine;

namespace ToolsACG.Networking
{
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "ScriptableObjects/ToolsACG/NetworkSettings")]
    public class NetworkSettings : ScriptableObject
    {
        [Header("Environment")]
        [SerializeField] private NetworkManager.Environment _environment;
        public NetworkManager.Environment Environment { get { return _environment; } set { _environment = value; } }

        [Header("API Urls")]
        [SerializeField] private string _developmentUrl;
        [SerializeField] private string _productionUrl;
        [SerializeField] private string _integrationUrl;
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
        [SerializeField] private int _timeOut;
        public int TimeOut { get { return _timeOut; } set { _timeOut = value; } }
    }
}
