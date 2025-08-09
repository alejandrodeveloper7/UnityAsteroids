using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolsACG.ApiCaller
{
    public class GeneralQueueService : ApiCaller<GeneralQueueService>
    {
        public void CleanGeneralQueue()
        {
            CleanQueue();
        }

        public async Task<Q> AddJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, int pPort = 0, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
          return await EnqueueJsonRequest<Q>(pEndPoint, pRequest, pPort, pMethod);
        }

        public async Task<Q> AddUrlParamRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, int pPort = 0, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
           return await EnqueueUrlParamRequest<Q>(pEndPoint, pParameter, pBody, pPort, pMethod);
        }

        public async Task<string> AddSimpleRequest(string pEndPoint, RequestEmpty pRequest, int pPort = 0, HttpMethod pMethod = HttpMethod.Post)
        {
            return await EnqueuSimpleRequest(pEndPoint, pRequest, pPort, pMethod);
        }
    }
}
