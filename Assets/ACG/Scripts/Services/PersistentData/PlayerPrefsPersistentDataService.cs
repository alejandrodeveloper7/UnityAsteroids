using Newtonsoft.Json;
using ACG.Tools.Runtime.PlayerPrefsExplorer.Services;
using ACG.Tools.Runtime.ServicesCreator.Bases;

namespace ACG.Scripts.Services
{
    public class PlayerPrefsPersistentDataService : InstancesServiceBase, IPersistentDataService
    {
        #region Fields

        // TODO: Declare your fields here

        #endregion

        #region Constructors

        public PlayerPrefsPersistentDataService()
        {
            Initialize();
        }

        #endregion

        #region Initialization     

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

        #region Set

        public void SetString(string key, string value)
        {
            PlayerPrefsService.SetString(key, value);
        }
        public void SetInt(string key, int value)
        {
            PlayerPrefsService.SetInt(key, value);
        }
        public void SetFloat(string key, float value)
        {
            PlayerPrefsService.SetFloat(key, value);
        }
        public void SetObject<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefsService.SetString(key, json);
        }

        #endregion

        #region Get

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefsService.GetString(key, defaultValue);
        }
        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefsService.GetInt(key, defaultValue);
        }
        public float GetFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefsService.GetFloat(key, defaultValue);
        }
        public T GetObject<T>(string key, T defaultValue = default)
        {
            if (PlayerPrefsService.HasKey(key) is false)
                return defaultValue;

            string json = PlayerPrefsService.GetString(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region Check

        public bool HasKey(string key)
        {
            return PlayerPrefsService.HasKey(key);
        }

        #endregion

        #region Clear

        public void Delete(string key)
        {
            PlayerPrefsService.DeleteKey(key);
        }
        public void Clear()
        {
            PlayerPrefsService.DeleteAllKeys();
        }

        #endregion
    }
}