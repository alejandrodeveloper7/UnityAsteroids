using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using ToolsACG.Core.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.UI.Controllers
{
    public class LeaderboardRowController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private Image _panel;
        [SerializeField] private Image _medal;
        [Space]
        [SerializeField] private TextMeshProUGUI _positionText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("References")]
        [Inject] private readonly SO_LeaderboardRowConfiguration _configuration;

        #endregion

        #region Initialization

        public void SetData(int position, LeaderboardEntry data, bool isPlayerScore)
        {
            ConfigureScore(data);
            ConfigurePosition(position);
            ConfigureName(data, isPlayerScore);
            
            _ = PlayEnterAnimation(position);
        }

        #endregion

        #region Configuration

        private void ConfigureScore(LeaderboardEntry data)
        {
            _scoreText.text = data.Score.ToString("N0");
        }

        private void ConfigurePosition(int position)
        {
            Sprite medalSprite = GetMedalSprite(position);
            bool hasMedal = medalSprite != null;

            _medal.gameObject.SetActive(hasMedal);
            _positionText.gameObject.SetActive(!hasMedal);

            if (hasMedal)
                _medal.sprite = medalSprite;
            else
                _positionText.text = $"{position}#";
        }

        private void ConfigureName(LeaderboardEntry data, bool isPlayerScore)
        {
            _nameText.text = data.Name;

            if (isPlayerScore)
                _panel.color = _configuration.PlayerRowColor;
            else
                _panel.color = _configuration.DefaultRowColor;
        }

        #endregion

        #region Animation

        private async Task PlayEnterAnimation(int position)
        {
            transform.localScale = Vector3.zero;
            await TimingUtils.WaitSeconds(position * _configuration.EnterRowAnimationDelay);

            Vector3 targetScale = GetPositionScale(position);
            transform.DOScale(targetScale, _configuration.EnterRowAnimationDuration);
        }

        #endregion

        #region Utility

        private Sprite GetMedalSprite(int position)
        {
            return position switch
            {
                1 => _configuration.FirstPositionMedal,
                2 => _configuration.SecondPositionMedal,
                3 => _configuration.ThirdPositionMedal,
                _ => null
            };
        }

        private Vector3 GetPositionScale(int position)
        {
            return position switch
            {
                1 => _configuration.FirstPositionScale,
                2 => _configuration.SecondPositionScale,
                3 => _configuration.ThirdPositionScale,
                _ => _configuration.DefaultPositionScale
            };
        }

        #endregion
    }
}