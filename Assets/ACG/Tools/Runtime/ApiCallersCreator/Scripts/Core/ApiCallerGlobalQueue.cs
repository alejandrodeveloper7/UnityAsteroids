using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;

namespace ACG.Tools.Runtime.ApiCallersCreator.Core
{
    public class ApiCallerGlobalQueue : ApiCallerBase<ApiCallerGlobalQueue>
    {
        #region Functionality

        public async Task<Q> AddJsonRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty request,  HttpMethod method = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
            return await EnqueueJsonRequest<Q>(configuration, endPoint, request, method);
        }

        public async Task<Q> AddUrlParamRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, string body,  HttpMethod method = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
            return await EnqueueUrlParamRequest<Q>(configuration, endPoint, body, method);
        }

        public async Task<string> AddSimpleRequest(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty request, HttpMethod method = HttpMethod.Post)
        {
            return await EnqueuSimpleRequest(configuration, endPoint, request, method);
        }

        #endregion
    }
}
