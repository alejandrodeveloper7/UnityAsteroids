using ToolsACG.Networking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ToolsACG.Services
{
    public class Service<T> where T : class, new()
    {
        #region Fields

        protected Queue<Action> ExecutionQueue = new Queue<Action>();

        protected bool IsSending;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }

        #endregion

        #region Log

        protected void LogAPI(string pUrl)
        {
            Debug.LogFormat("<color=yellow>API ---> {0}</color>", pUrl);
        }

        protected void LogGetRequest(string pUrl)
        {
            Debug.LogFormat("<color=Cyan>GET REQUEST --- {0} </color>", pUrl);
        }

        protected void LogRequest(object pRequest, string pUrl)
        {
            Debug.LogFormat("<color=Cyan>REQUEST --- {0} ---></color> {1}", pUrl, pRequest);
        }

        protected void LogResponse(object pResponse, string pUrl)
        {
            Debug.LogFormat("<color=lime>RESPONSE --- {0} ---></color> {1}", pUrl, pResponse);
        }

        protected void LogError(string pText)
        {
            Debug.LogErrorFormat("<color=red>[ERROR] --- {0} </color>", pText);
        }

        protected void LogReponseTime(string pUrl, float pTime)
        {
            Debug.LogFormat("<color=orange>TIME RESPONE {0} ms --- {1}</color>", pTime, pUrl);
        }

        #endregion

        #region Apis management

        protected async Task SendUrlParamsRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, Action<Q> pCallback, int pPort, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false) where Q : ResponseEmpty
        {
            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)            
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);
            

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);
            string completeUrlWithParameters = BuildUrl(completeUrl, pParameter);

            LogAPI(completeUrlWithParameters);

            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrlWithParameters);
                    LogGetRequest(pEndPoint);
                    break;



                case HttpMethod.Post:
                case HttpMethod.Put:
                    if (string.IsNullOrEmpty(pBody))
                    {
                        request = UnityWebRequest.PostWwwForm(completeUrlWithParameters, "");
                        LogRequest("", pEndPoint);
                    }
                    else
                    {
                        request = UnityWebRequest.PostWwwForm(completeUrlWithParameters, pBody);
                        SetJsonBody(request, pBody);
                        LogRequest(pBody, pEndPoint);
                    }
                    break;

            };

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOut;
            AddHeaders(request);

            string respose = "";
            Q responseObj = default;

            float ping = Time.time;

            request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            LogReponseTime(completeUrlWithParameters, Mathf.RoundToInt((pong - ping) * 1000));

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;
                LogResponse(respose, pEndPoint);
                responseObj = JsonConvert.DeserializeObject<Q>(respose);

                if (pCallback?.Target == null)
                {
                    LogError(string.Format(format: "Origin of call null - {0}", completeUrl));
                    pCallbackError?.Invoke(responseObj);
                }
                else if (responseObj == null)
                {
                    LogError(string.Format("Response is null - {0}", completeUrl));
                    pCallbackError?.Invoke(responseObj);
                }
                else
                {
                    pCallback?.Invoke(responseObj);
                }
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrlWithParameters, request.responseCode, request.error));
                pCallbackError?.Invoke(responseObj);
            }
            IsSending = false;

            if (pEnqueued)
                CheckRequests();
        }
        protected async Task SendJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, Action<Q> pCallback, int pPort, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false) where Q : ResponseEmpty
        {

            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)            
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);            

            LogAPI(pEndPoint);

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);

            string data = "";
            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrl);
                    LogGetRequest(pEndPoint);
                    break;



                case HttpMethod.Post:
                    request = new UnityWebRequest(completeUrl, "POST");

                    data = JsonConvert.SerializeObject(pRequest);
                    LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;



                case HttpMethod.Put:
                    request = new UnityWebRequest(completeUrl, "PUT");

                    data = JsonConvert.SerializeObject(pRequest);
                    LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;

            };

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOut;
            AddHeaders(request);

            string respose = "";
            Q responseObj = default;

            float ping = Time.time;

            request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            LogReponseTime(pEndPoint, Mathf.RoundToInt((pong - ping) * 1000));

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;

                LogResponse(respose, pEndPoint);

                responseObj = JsonConvert.DeserializeObject<Q>(respose);

                if (pCallback?.Target == null)
                {
                    LogError(string.Format(format: "Origin of call null - {0}", completeUrl));
                    pCallbackError?.Invoke(responseObj);
                }
                else if (responseObj == null)
                {
                    LogError(string.Format("Response is null - {0}", completeUrl));
                    pCallbackError?.Invoke(responseObj);
                }
                else
                {
                    pCallback?.Invoke(responseObj);
                }
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrl, request.responseCode, request.error));
                pCallbackError?.Invoke(responseObj);
            }

            IsSending = false;

            if (pEnqueued)
                CheckRequests();
        }
        protected async Task SendSimpleRequest(string pEndPoint, RequestEmpty pRequest, Action<string> pCallback, int pPort, Action<string> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false)
        {
            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)            
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);            

            LogAPI(pEndPoint);

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);

            string data = "";
            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrl);
                    LogGetRequest(pEndPoint);
                    break;



                case HttpMethod.Post:
                    request = new UnityWebRequest(completeUrl, "POST");

                    data = JsonConvert.SerializeObject(pRequest);
                    LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;



                case HttpMethod.Put:
                    request = new UnityWebRequest(completeUrl, "PUT");

                    data = JsonConvert.SerializeObject(pRequest);
                    LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;

            };

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOut;
            AddHeaders(request);

            string respose = "";

            float ping = Time.time;

            request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            LogReponseTime(pEndPoint, Mathf.RoundToInt((pong - ping) * 1000));

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;

                LogResponse(respose, pEndPoint);

                if (pCallback?.Target == null)
                {
                    LogError(string.Format(format: "Origin of call null - {0}", completeUrl));
                    pCallbackError?.Invoke(respose);
                }
                else if (respose == null)
                {
                    LogError(string.Format("Response is null - {0}", completeUrl));
                    pCallbackError?.Invoke(respose);
                }
                else
                {
                    pCallback?.Invoke(respose);
                }
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrl, request.responseCode, request.error));
                pCallbackError?.Invoke(respose);
            }

            IsSending = false;

            if (pEnqueued)
                CheckRequests();
        }

        protected virtual void EnqueueJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, Action<Q> pCallback, int pPort, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty
        {
            ExecutionQueue.Enqueue(() => _ = SendJsonRequest<Q>(pEndPoint, pRequest, pCallback, pPort, pCallbackError, pMethod, true));

            if (IsSending is false)
                CheckRequests();
        }
        protected virtual void EnqueueUrlParamRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, Action<Q> pCallback, int pPort = 0, Action<Q> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty
        {
            ExecutionQueue.Enqueue(() => _ = SendUrlParamsRequest<Q>(pEndPoint, pParameter, pBody, pCallback, pPort, pCallbackError, pMethod, true));

            if (IsSending is false)
                CheckRequests();
        }
        protected virtual void EnqueuSimpleRequest(string pEndPoint, RequestEmpty pRequest, Action<string> pCallback, int pPort, Action<string> pCallbackError = null, HttpMethod pMethod = HttpMethod.Post)
        {
            ExecutionQueue.Enqueue(() => _ = SendSimpleRequest(pEndPoint, pRequest, pCallback, pPort, pCallbackError, pMethod, true));

            if (IsSending is false)
                CheckRequests();
        }

        private void CheckRequests()
        {
            if (ExecutionQueue.Count > 0)
                ExecutionQueue.Dequeue().Invoke();
        }
        protected virtual void CleanQueue()
        {
            ExecutionQueue.Clear();
            IsSending = false;
            Debug.Log(string.Format("---------------  {0} QUEUE CLEANED  ---------------", this.GetType().Name));
        }

        #endregion

        #region Utility

        protected string BuildUrl(string pUrl, Dictionary<string, string> pParameters)
        {
            if (pParameters != null && pParameters.Count > 0)
            {
                string queryString = string.Join("&", pParameters.Select(pair =>
                  Uri.EscapeDataString(pair.Key) + "=" + Uri.EscapeDataString(pair.Value)));

                pUrl += (pUrl.Contains("?") ? "&" : "?") + queryString;
            }
            return pUrl;
        }

        protected void SetJsonBody(UnityWebRequest pRequest, string pJsonBody)
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(pJsonBody);
            pRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            pRequest.downloadHandler = new DownloadHandlerBuffer();
            pRequest.SetRequestHeader("Content-Type", "application/json");
        }

        protected void AddHeaders(UnityWebRequest pRequest)
        {
            //TODO Implement if needed
        }

        #endregion
    }

    #region Models and enums

    [Serializable]
    public class RequestEmpty
    {
    }
    [Serializable]
    public class ResponseEmpty
    {

    }


    [Serializable]
    public class RequestBase : RequestEmpty
    {
    }
    [Serializable]
    public class ResponseBase : ResponseEmpty
    {
        public bool success;
        public ErrorData errorData;
    }


    [Serializable]
    public class ErrorData
    {
        public string errorCode;
        public string message;
    }

    public enum HttpMethod
    {
        Get = 0,
        Post = 10,
        Put = 20,
    }

    #endregion
}
