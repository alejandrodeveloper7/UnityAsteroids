namespace ToolsACG.Core.Managers
{
    public interface IScreenManager
    {
        void ApplyFrameRate();
        void ApplyInitialOrientation();
        void ApplyOrientationConfiguration();
        void PreventSleep();
        void RestoreSystemSleepTimeOut();
    }
}