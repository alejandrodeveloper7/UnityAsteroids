using ACG.Scripts.Services;
using Asteroids.Core.ScriptableObjects.Configurations;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Controllers
{
    public class ScenesController
    {
        #region Fields

        [Header("Data")]
        [Inject] private readonly SO_ScenesConfiguration _scenesConfiguration;

        #endregion

        #region Scenes Load

        public async Task LoadGameplayAdditiveScenes()
        {
            await AdditiveScenesService.LoadAdditiveScenes(_scenesConfiguration.DesktopSceneDependencies);
        }

        public async Task UnloadGameplayAdditiveScenes()
        {
            await AdditiveScenesService.UnloadAdditiveScenes(_scenesConfiguration.DesktopSceneDependencies);
        }

        #endregion
    }
}