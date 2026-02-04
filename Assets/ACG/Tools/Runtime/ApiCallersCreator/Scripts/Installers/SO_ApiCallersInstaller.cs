using ACG.Tools.Runtime.ApiCallers.Pokemon;
using UnityEngine;
using Zenject;

namespace ACG.Tools.Runtime.ApiCallers.Installers
{
    [CreateAssetMenu(fileName = "ApiCallersInstaller", menuName = "Installers/ApiCallers")]
    public class SO_ApiCallersInstaller : ScriptableObjectInstaller<SO_ApiCallersInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            //Api callers            
            Container.Bind<IPokemonApiCaller>().To<PokemonApiCaller>().FromInstance(PokemonApiCaller.Instance);
            Container.Bind<IPokemonApiService>().To<PokemonApiService>().AsSingle();
            Container.Bind<PokemonApiContainer>().AsSingle();
        }

        #endregion
    }
}