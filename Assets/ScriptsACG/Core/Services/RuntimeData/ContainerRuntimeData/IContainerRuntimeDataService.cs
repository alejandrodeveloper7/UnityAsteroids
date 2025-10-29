namespace ToolsACG.Core.Services
{
    public interface IContainerRuntimeDataService : IRuntimeDataService
    {
        RuntimeDataContainer Data { get; }
    }
}