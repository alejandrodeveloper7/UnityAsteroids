namespace ACG.Scripts.Managers
{
    public interface ICursorManager
    {
        public void SetCursorToDefault();
        public void SetCursorToClicked();

        void SetUICursor();
        void SetGameplayCursor();
    }
}