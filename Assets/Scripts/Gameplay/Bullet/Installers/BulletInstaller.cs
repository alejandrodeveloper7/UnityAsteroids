using Asteroids.Gameplay.Bullets.Controllers;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Intallers
{
    public class BulletInstaller : MonoInstaller
    {      
        public override void InstallBindings()
        {
            Container.Bind<BulletController>().FromComponentSibling();
            Container.Bind<BulletVisualsController>().FromComponentSibling();
            Container.Bind<BulletPhysicsController>().FromComponentSibling();
        }
    }
}