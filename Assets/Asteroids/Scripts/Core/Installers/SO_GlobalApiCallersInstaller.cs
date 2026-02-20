using ACG.Tools.Runtime.ApiCallers.Pokemon;
using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "GlobalApiCallersInstaller", menuName = "Installers/GlobalApiCallers")]
    public class SO_GlobalApiCallersInstaller : ScriptableObjectInstaller<SO_GlobalApiCallersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            //Game Apicallers            
            Container.Bind<IDreamloLeaderboardApiCaller>().To<DreamloLeaderboardApiCaller>().FromInstance(DreamloLeaderboardApiCaller.Instance);
            Container.Bind<IDreamloLeaderboardApiService>().To<DreamloLeaderboardApiService>().AsSingle();
            Container.Bind<DreamloLeaderboardApiContainer>().AsSingle();

            //Example ApiCaller
            Container.Bind<IPokemonApiCaller>().To<PokemonApiCaller>().FromInstance(PokemonApiCaller.Instance);
            Container.Bind<IPokemonApiService>().To<PokemonApiService>().AsSingle();
            Container.Bind<PokemonApiContainer>().AsSingle();
        }

        #endregion
    }
}