namespace ToolsACG.Core.Managers
{
    public interface ICursorManager
    {
        void SetCursorToDefault();
        void SetCursorToClicked();

        void SetUICursor();
        void SetGameplayCursor();
    }
}