using ACG.Tools.Runtime.ServicesCreator.Bases;
using Zenject;

namespace Asteroids.Core.Services
{
    public class ContainerRuntimeDataService : InstancesServiceBase, IContainerRuntimeDataService
    {
        #region Fields

        public RuntimeDataContainer Data { get; private set; }

        #endregion

        #region Constructors

        [Inject]
        public ContainerRuntimeDataService(RuntimeDataContainer container)
        {
            Data = container;
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion       
    }
}