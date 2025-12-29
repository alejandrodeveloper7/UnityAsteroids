using Asteroids.Gameplay.Backgrounds.Controllers;

using Zenject;

namespace Asteroids.Gameplay.Backgrounds.Intallers
{
    public class BackgroundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BackgroundVisualsController>().FromComponentSibling();
        }
    }
}