namespace ToolsACG.Core.Services
{
    public interface IPersistentDataService
    {
        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetString(string key, string value);
        void SetObject<T>(string key, T data);

        int GetInt(string key, int defaultValue = 0);
        float GetFloat(string key, float defaultValue = 0f);
        string GetString(string key, string defaultValue = "");
        T GetObject<T>(string key, T defaultValue = default);

        bool HasKey(string key);
        void Delete(string key);
        void Clear();
    }
}
