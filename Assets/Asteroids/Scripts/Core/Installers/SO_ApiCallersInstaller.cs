using ACG.Tools.Runtime.ApiCallers.Pokemon;
using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Intallers
{
    [CreateAssetMenu(fileName = "ApiCallersInstaller", menuName = "Installers/ApiCallers")]
    public class SO_ApiCallersInstaller : ScriptableObjectInstaller<SO_ApiCallersInstaller>
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