using DG.Tweening;
using ToolsACG.Core.Utils;
using UnityEngine;

namespace Asteroids.Gameplay.Backgrounds.Controllers
{
    public class BackgroundVisualsController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private SpriteRenderer _backgroundSprite;

        #endregion

        #region Alpha Management

        public void SetAlphaValue(float value)
        {
            _backgroundSprite.color = ColorUtils.GetColorWithAlpha(_backgroundSprite.color, value);
        }

        public void DoFadeTransition(float value, float duration)
        {
            _backgroundSprite.DOKill();
            _backgroundSprite.DOFade(value, duration);
        }

        #endregion
    }
}