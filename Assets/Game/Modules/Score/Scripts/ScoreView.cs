using TMPro;
using UnityEngine;

namespace ToolsACG.Scenes.Score
{
    public interface IScoreView
    {
        void TurnGeneralContainer(bool pState);
        void SetScore(int pValue);
    }

    public class ScoreView : ModuleView, IScoreView
    {
        #region Fields  

        [SerializeField] private GameObject _generalContainer;
        [SerializeField] private TextMeshProUGUI _scoreText;

        #endregion

        #region View Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }            

        public void SetScore(int pvalue)
        {
            _scoreText.text = pvalue.ToString();
        }

        #endregion
    }
}