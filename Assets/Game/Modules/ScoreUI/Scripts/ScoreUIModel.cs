using System;

namespace ToolsACG.Scenes.ScoreUI
{
    public class ScoreUIModel : ModuleModel
    {
        #region Fields

        private bool _alive; public bool Alive { get { return _alive; } }
        private int _score; public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnScoreUpdated(_score);
            }
        }

        #endregion

        #region Actions

        public event Action<int> OnScoreUpdated;

        #endregion

        #region Constructors

        public ScoreUIModel()
        {
        }

        #endregion

        #region public Methods

        public void SetAlive(bool pState)
        {
            _alive = pState;
        }

        #endregion
    }
}