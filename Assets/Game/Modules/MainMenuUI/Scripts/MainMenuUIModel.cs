using System;

namespace ToolsACG.Scenes.MainMenuUI
{
    public class MainMenuUIModel : ModuleModel
    {
        #region Fields

        private string _userName; public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnUserNameUpdated(_userName);
            }
        }

        #endregion

        #region Actions

        public event Action<string> OnUserNameUpdated;

        #endregion

        #region Constructors

        public MainMenuUIModel()
        {
        }

        #endregion
    }
}