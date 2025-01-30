using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ToolsACG.Services
{
    public class GeneralQueueService : Service<GeneralQueueService>
    {
        public void CleanGeneralQueue()
        {
            CleanQueue();
        }

        public void AddJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, Action<Q> pCallback, int pPort = 0, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
            EnqueueJsonRequest(pEndPoint, pRequest, pCallback, pPort, pCallbackError, pMethod);
        }

        public void AddUrlParamRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, Action<Q> pCallback, int pPort = 0, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty, new()
        {
            EnqueueUrlParamRequest(pEndPoint, pParameter, pBody, pCallback, pPort, pCallbackError, pMethod);
        }

        public void AddSimpleRequest(string pEndPoint, RequestEmpty pRequest, Action<string> pCallback, int pPort = 0, Action<string> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post)
        {
            EnqueuSimpleRequest(pEndPoint, pRequest, pCallback, pPort, pCallbackError, pMethod);
        }
    }
}
