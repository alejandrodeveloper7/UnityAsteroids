namespace Asteroids.Core.Services
{
    public interface IContainerRuntimeDataService : IRuntimeDataService
    {
        RuntimeDataContainer Data { get; }
    }
}