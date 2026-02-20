using ACG.Scripts.Utilitys;
using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Gameplay.Asteroids.Controllers;
using Asteroids.Gameplay.General.OnContact;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.Asteroids.Intallers
{
    public class AsteroidInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AsteroidController>().FromComponentSibling();
            Container.Bind<AsteroidVisualsController>().FromComponentSibling();
            Container.Bind<AsteroidHealthController>().FromComponentSibling();
            Container.Bind<AsteroidMovementController>().FromComponentSibling();
            Container.Bind<AsteroidPhysicsController>().FromComponentSibling();
            Container.Bind<AsteroidSoundController>().FromComponentSibling();

            Container.Bind<DamageOnContact>().FromComponentSibling();
            Container.Bind<ScreenEdgeTeleport>().FromComponentSibling();
       
            Container.Bind<SpriteRenderer>().FromComponentSibling();
            Container.Bind<Rigidbody2D>().FromComponentSibling();
            Container.Bind<PolygonCollider2D>().FromComponentSibling();
        }
    }
}