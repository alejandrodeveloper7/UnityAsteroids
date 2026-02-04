using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Gameplay.FloatingText.Controllers;
using Zenject;

namespace Asteroids.Gameplay.FloatingText.Intallers
{
    public class FloatingTextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FloatingTextVisualsController>().FromComponentSibling();

            Container.Bind<PooledGameObjectController>().FromComponentSibling();
        }
    }
}