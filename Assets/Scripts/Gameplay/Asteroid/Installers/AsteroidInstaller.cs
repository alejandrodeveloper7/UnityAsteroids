using Asteroids.Gameplay.Asteroids.Controllers;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Intallers
{
    public class AsteroidInstaller : MonoInstaller
    {
        // We don't need separate installers for different variants of the prefab, 
        // so this installer is placed in the ApplicationContext instead of on each prefab.

        // For this same reason is a MonoInstaller and not a ScriptableObjectInstaller,
        // because in the future it would be placed in the prefabs.

        public override void InstallBindings()
        {
            Container.Bind<AsteroidController>().FromComponentSibling();
            Container.Bind<AsteroidMovementController>().FromComponentSibling();
            Container.Bind<AsteroidHealthController>().FromComponentSibling();
            Container.Bind<AsteroidVisualsController>().FromComponentSibling();
        }
    }
}