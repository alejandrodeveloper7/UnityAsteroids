using Asteroids.Gameplay.Backgrounds.Controllers;
using Asteroids.Gameplay.FloatingText.Controllers;
using Asteroids.Gameplay.General.OnContact;
using ToolsACG.Core.Utilitys;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;
using Zenject;

namespace Asteroids.Gameplay.General.Intallers
{
    public class PrefabsGeneralInstaller : MonoInstaller
    {
        // We don't need separate installers for different variants of the prefab, 
        // so this installer is placed in the ApplicationContext instead of on each prefab.

        // For this same reason is a MonoInstaller and not a ScriptableObjectInstaller,
        // because in the future it would be placed in the prefabs.

        // These bindings are common to all prefabs or one Bind only, so they are defined here 
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
