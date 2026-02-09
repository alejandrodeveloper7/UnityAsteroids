using ACG.Scripts.Utilitys;
using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Gameplay.Bullets.Controllers;
using Asteroids.Gameplay.General.OnContact;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Bullets.Intallers
{
    public class BulletInstaller : MonoInstaller
    {      
        public override void InstallBindings()
        {
            Container.Bind<BulletController>().FromComponentSibling();
            Container.Bind<BulletVisualsController>().FromComponentSibling();
            Container.Bind<BulletLifeTimeController>().FromComponentSibling();
            Container.Bind<BulletMovementController>().FromComponentSibling();
            Container.Bind<BulletPhysicsController>().FromComponentSibling();
            Container.Bind<BulletSoundController>().FromComponentSibling();

            Container.Bind<PushOnContact>().FromComponentSibling();
            Container.Bind<DamageOnContact>().FromComponentSibling();
            Container.Bind<PooledGameObjectController>().FromComponentSibling();
            Container.Bind<ScreenEdgeTeleport>().FromComponentSibling();
          
            Container.Bind<SpriteRenderer>().FromComponentSibling();
            Container.Bind<Rigidbody2D>().FromComponentSibling();
            Container.Bind<BoxCollider2D>().FromComponentSibling();
        }
    }
}