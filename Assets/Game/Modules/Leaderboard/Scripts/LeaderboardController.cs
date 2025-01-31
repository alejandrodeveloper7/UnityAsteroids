using UnityEngine;

namespace ToolsACG.Scenes.Leaderboard
{
    [RequireComponent(typeof(LeaderboardView))]
    public class LeaderboardController : ModuleController
    {
        #region Private Fields

        private ILeaderboardView _view;
        private LeaderboardModel _data;

        #endregion

        #region Properties

        public LeaderboardModel Model
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        #region Protected Methods

        protected override void Awake()
        {
            _view = GetComponent<ILeaderboardView>();
            base.Awake();
            _data = new LeaderboardModel();

            Initialize();
        }

        protected override void RegisterActions()
        {
            // TODO: initialize dictionaries with actions for buttons, toggles and dropdowns.      
        }

        protected override void SetData()
        {
            // TODO: initialize model with services data (if it's not initialized externally using Data property).
            // TODO: call view methods to display data.
        }

        #endregion

        #region Monobehaviour



        #endregion

        #region Bus callbacks



        #endregion

        private void Initialize()
        {
            _view.TurnGeneralContainer(false);
        }
    }
}