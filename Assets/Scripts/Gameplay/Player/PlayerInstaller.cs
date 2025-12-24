using Asteroids.Gameplay.Player.Controllers;
using Zenject;

namespace Asteroids.Gameplay.Player.Intallers
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerController>().FromComponentSibling();
            Container.Bind<PlayerHealthController>().FromComponentSibling();
            Container.Bind<PlayerPhysicsController>().FromComponentSibling();
            Container.Bind<PlayerMovementController>().FromComponentSibling();
            Container.Bind<PlayerVisualsController>().FromComponentSibling();
        }
    }
}