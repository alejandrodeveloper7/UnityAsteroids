using Asteroids.Core.ScriptableObjects.Configurations;
using DG.Tweening;
using System.Threading.Tasks;
using ACG.Tools.Runtime.Pooling.Gameplay;
using UnityEngine;
using Zenject;
using ACG.Core.Extensions;

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
            Vector3 randomDir = Random.insideUnitCircle.normalized * _configuration.MovementDistanceRange.GetRandom();
            Vector3 targetPosition = transform.localPosition + randomDir.FlattenZ();

            Task visualTask = _floatingTextVisuals.PlayDisplaySequence(_configuration);
            Task movementTask = transform.DOLocalMove(targetPosition, _configuration.TotalLifeTime)
                .SetEase(Ease.OutQuad)
                .AsyncWaitForCompletion();

            await Task.WhenAll(visualTask, movementTask);

            _pooledGameObject.RecycleGameObject();
        }

        #endregion

    }
}