using Asteroids.Gameplay.Backgrounds.Controllers;
using Asteroids.Gameplay.FloatingText.Controllers;
using Asteroids.Gameplay.General.OnContact;
using ToolsACG.Core.Utilitys;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.General.Intallers
{
    public class GameplayGeneralInstaller : MonoInstaller
    {      
        // These bindings are common to several prefabs or one Bind only, so they are defined here 
        // instead of in each prefab's installer.

        public override void InstallBindings()
        {
            //Common
            Container.Bind<DamageOnContact>().FromComponentSibling();
            Container.Bind<PushOnContact>().FromComponentSibling();            
            Container.Bind<PooledGameObject>().FromComponentSibling();
            Container.Bind<ScreenEdgeTeleport>().FromComponentSibling();

            Container.Bind<SpriteRenderer>().FromComponentSibling();
            Container.Bind<Rigidbody2D>().FromComponentSibling();
            Container.Bind<PolygonCollider2D>().FromComponentSibling();
            Container.Bind<BoxCollider2D>().FromComponentSibling();

            //One Bind only
            Container.Bind<BackgroundVisualsController>().FromComponentSibling();
            Container.Bind<FloatingTextVisualsController>().FromComponentSibling();
        }
    }
}
