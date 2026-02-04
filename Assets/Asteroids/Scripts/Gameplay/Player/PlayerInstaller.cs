using ACG.Scripts.Utilitys;
using ACG.Tools.Runtime.Pooling.Gameplay;
using Asteroids.Gameplay.Player.Controllers;
using UnityEngine;
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

            Container.Bind<PooledGameObjectController>().FromComponentSibling();
            Container.Bind<ScreenEdgeTeleport>().FromComponentSibling();
            
            Container.Bind<SpriteRenderer>().FromComponentSibling();
            Container.Bind<Rigidbody2D>().FromComponentSibling();
            Container.Bind<PolygonCollider2D>().FromComponentSibling();
        }
    }
}