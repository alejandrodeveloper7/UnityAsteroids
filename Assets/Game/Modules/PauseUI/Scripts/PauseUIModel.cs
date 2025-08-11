
namespace ToolsACG.Scenes.PauseUI
{
    public class PauseUIModel : ModuleModel
    {
        #region Fields and Properties

        private bool _isInPause; public bool InPause
        {
            get { return _isInPause; }
            set { _isInPause = value; }
        }

        #endregion

        #region Actions

        //public event Action OnSomething;
        //public event Action<int> OnSomethingWithParameter;

        #endregion

        #region Constructors

        public PauseUIModel()
        {
        }

        #endregion
    }
}