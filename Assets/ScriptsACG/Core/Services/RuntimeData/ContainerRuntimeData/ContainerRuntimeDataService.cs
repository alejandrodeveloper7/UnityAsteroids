using System;
using ToolsACG.ServicesCreator.Bases;
using Zenject;

namespace ToolsACG.Core.Services
{
    public class ContainerRuntimeDataService : InstancesServiceBase, IContainerRuntimeDataService
    {
        #region Fields

        public RuntimeDataContainer Data { get; private set; }

        public string AuthToken { get { return Data.AuthToken; } set { Data.AuthToken = value; } }
        public string RefreshToken { get { return Data.RefreshToken; } set { Data.RefreshToken = value; } }
        public DateTime TokenExpiration { get { return Data.TokenExpiration; } set { Data.TokenExpiration = value; } }

        #endregion

        #region Constructors

        [Inject]
        public ContainerRuntimeDataService(RuntimeDataContainer container)
        {
            Data = container;
            
            Initialize();
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
