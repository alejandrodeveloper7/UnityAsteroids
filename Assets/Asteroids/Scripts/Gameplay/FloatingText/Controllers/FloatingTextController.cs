using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using System.Threading.Tasks;
using ACG.Tools.Runtime.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.FloatingText.Controllers
{
    [RequireComponent(typeof(FloatingTextVisualsController))]
    [RequireComponent(typeof(PooledGameObjectController))]

    public class FloatingTextController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Inject] private readonly FloatingTextVisualsController _floatingTextVisuals;
        [Inject] private readonly PooledGameObjectController _pooledGameObject;

        [Header("Data")]
        [Inject] private readonly SO_FloatingTextConfiguration _configuration;

        #endregion

        #region Initialization

        public void Initialize(string text)
        {
            _floatingTextVisuals.SetData(text, _configuration.TextColor);
            _ = StartDisplayProcess();
        }

        #endregion

        #region Functionality

        private async Task StartDisplayProcess()
        {
            Vector3 randomDir = Random.insideUnitCircle.normalized * Random.Range(_configuration.MinMovementDistance, _configuration.MaxMovementDistance);
            Vector3 targetPosition = transform.localPosition + new Vector3(randomDir.x, randomDir.y, 0);

            Task visualTask = _floatingTextVisuals.PlayDisplaySequence(_configuration);
            Task movementTask = transform.DOLocalMove(targetPosition, _configuration.LifeTime + _configuration.TransitionDuration * 2)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();

            await Task.WhenAll(visualTask, movementTask);

            _pooledGameObject.RecycleGameObject();
        }

        #endregion

    }
}