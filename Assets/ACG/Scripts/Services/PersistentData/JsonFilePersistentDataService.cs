using Newtonsoft.Json;
using System.IO;
using ACG.Tools.Runtime.ServicesCreator.Bases;

namespace ACG.Scripts.Services
{
    public class JsonFilePersistentDataService : InstancesServiceBase, IPersistentDataService
    {
        #region Fields

        private readonly string basePath;

        #endregion

        #region Constructors

        public JsonFilePersistentDataService(string folderName = "SavedData")
        {
            basePath = Path.Combine(UnityEngine.Application.persistentDataPath, folderName);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

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
            SetObject(key, value);
        }
        public void SetInt(string key, int value)
        {
            SetObject(key, value);
        }
        public void SetFloat(string key, float value)
        {
            SetObject(key, value);
        }
        public void SetObject<T>(string key, T data)
        {
            string path = GetPath(key);
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(path, json);
        }

        #endregion

        #region Get

        public int GetInt(string key, int defaultValue = 0)
        {
            return GetObject(key, defaultValue);
        }
        public float GetFloat(string key, float defaultValue = 0f)
        {
            return GetObject(key, defaultValue);
        }
        public string GetString(string key, string defaultValue = "")
        {
            return GetObject(key, defaultValue);
        }
        public T GetObject<T>(string key, T defaultValue = default)
        {
            string path = GetPath(key);

            if (!File.Exists(path))
                return defaultValue;

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region Check

        public bool HasKey(string key)
        {
            return File.Exists(GetPath(key));
        }

        #endregion

        #region Clear

        public void Delete(string key)
        {
            string path = GetPath(key);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void Clear()
        {
            if (Directory.Exists(basePath))
                Directory.Delete(basePath, true);

            Directory.CreateDirectory(basePath);
        }

        #endregion

        #region private methods

        private string GetPath(string key)
        {
            return Path.Combine(basePath, key + ".json");
        }

        #endregion
    }
}