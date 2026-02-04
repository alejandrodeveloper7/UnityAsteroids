namespace Asteroids.Core.Services
{
    public interface IKeyValueRuntimeDataService : IRuntimeDataService
    {
        void Set<T>(string key, T value);
        T Get<T>(string key, T defaultValue = default);

        void Clear();
    }
}