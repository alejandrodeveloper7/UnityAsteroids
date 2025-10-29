using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Asteroids.Gameplay.FloatingText.Controllers
{
    public class FloatingTextVisualsController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private TextMeshPro _text;

        [Header("Cache")]
        private Sequence _sequence;

        #endregion

        #region Initialization

        public void SetData(string text, Color color)
        {
            _text.text = text;
            _text.color = color;
        }

        #endregion

        #region Management

        public void SetAlpha(float value)
        {
            Color color = _text.color;
            color.a = value;
            _text.color = color;
        }

        public async Task PlayDisplaySequence(SO_FloatingTextConfiguration configuration)
        {
            StopDisplaySequence();
            _sequence = DOTween.Sequence();

            SetAlpha(0);
            transform.localScale = Vector3.zero;

            _sequence.Append(transform.DOScale(1, configuration.TransitionDuration));
            _sequence.Join(_text.DOFade(1, configuration.TransitionDuration));            
            _sequence.AppendInterval(configuration.LifeTime);
            _sequence.Append(_text.DOFade(0, configuration.TransitionDuration));
            _sequence.Join(transform.DOScale(0, configuration.TransitionDuration));

            await _sequence.Play().AsyncWaitForCompletion();
        }

        public void StopDisplaySequence()
        {
            _sequence?.Kill();
        }

        #endregion
    }
}