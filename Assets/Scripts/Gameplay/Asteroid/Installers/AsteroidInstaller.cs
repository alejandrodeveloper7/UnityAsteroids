using Asteroids.Gameplay.Asteroids.Controllers;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Intallers
{
    public class AsteroidInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AsteroidController>().FromComponentSibling();
            Container.Bind<AsteroidMovementController>().FromComponentSibling();
            Container.Bind<AsteroidHealthController>().FromComponentSibling();
            Container.Bind<AsteroidVisualsController>().FromComponentSibling();
        }
    }
}