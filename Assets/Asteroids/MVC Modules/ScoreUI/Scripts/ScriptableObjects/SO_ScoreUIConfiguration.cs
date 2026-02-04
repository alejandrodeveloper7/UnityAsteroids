using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using UnityEngine;

namespace Asteroids.MVC.ScoreUI.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ScoreUIConfiguration", menuName = "ScriptableObjects/ToolsACG/MVCModules/ScoreUIConfiguration")]
    public class SO_ScoreUIConfiguration : SO_MVCConfigurationBase
    {
        #region Values

        [Header("Score")]
        
        [SerializeField] private float _scoreIncreaseDuration;
        public float ScoreIncreaseDuration => _scoreIncreaseDuration;


        [Header("Feedback")]

        [SerializeField] private Vector3 _scorePanelFeedbackScale;
        public Vector3 ScorePanelFeedbackScale=> _scorePanelFeedbackScale;

        [SerializeField] private float _scorePanelFeedbackDuration;
        public float ScorePanelFeedbackDuration => _scorePanelFeedbackDuration;

        #endregion

        #region Methods

        // TODO: Declare your methods here

        #endregion
    }
}
