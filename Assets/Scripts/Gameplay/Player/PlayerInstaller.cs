using Asteroids.Gameplay.Player.Controllers;
using Zenject;

namespace Asteroids.Gameplay.Player.Intallers
{
    public class PlayerInstaller : MonoInstaller
    {
        // We don't need separate installers for different variants of the prefab, 
        // so this installer is placed in the ApplicationContext instead of on each prefab.

        // For this same reason is a MonoInstaller and not a ScriptableObjectInstaller,
        // because in the future it would be placed in the prefabs.

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