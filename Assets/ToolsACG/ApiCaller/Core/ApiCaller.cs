using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolsACG.Networking;
using UnityEngine;
using UnityEngine.Networking;

namespace ToolsACG.ApiCaller
{
    public class ApiCaller<T> where T : class, new()
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
            Debug.LogErrorFormat("<color=red>[ERROR] --- {0} --- </color>", pText);
        }

        protected void LogReponseTime(string pUrl, float pTime)
        {
            Debug.LogFormat("<color=orange>TIME RESPONE {0} ms --- {1}</color>", pTime, pUrl);
        }

        #endregion

        #region Apis management

        protected async Task<Q> SendUrlParamsRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, int pPort, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false) where Q : ResponseEmpty
        {
            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);
            string completeUrlWithParameters = BuildUrl(completeUrl, pParameter);

            if (NetworkManager.Instance.NetworkSetting.LogApiCall)
                LogAPI(completeUrlWithParameters);

            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrlWithParameters);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(pBody, pEndPoint);
                    break;


                case HttpMethod.Post:
                case HttpMethod.Put:
                    if (string.IsNullOrEmpty(pBody))
                    {
                        request = UnityWebRequest.PostWwwForm(completeUrlWithParameters, "");
                        if (NetworkManager.Instance.NetworkSetting.LogRequest)
                            LogRequest(pBody, pEndPoint);
                    }
                    else
                    {
                        request = UnityWebRequest.PostWwwForm(completeUrlWithParameters, pBody);
                        SetJsonBody(request, pBody);
                        if (NetworkManager.Instance.NetworkSetting.LogRequest)
                            LogRequest(pBody, pEndPoint);
                    }
                    break;
            }
            ;

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOutMs;
            AddHeaders(request);

            string respose = "";
            Q responseObj = default;

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkManager.Instance.NetworkSetting.LogResponseTime)
                LogReponseTime(completeUrlWithParameters, Mathf.RoundToInt((pong - ping) * 1000));


            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;
                if (NetworkManager.Instance.NetworkSetting.LogResponse)
                    LogResponse(respose, pEndPoint);

                try
                {
                    responseObj = JsonConvert.DeserializeObject<Q>(respose);
                }
                catch (Exception exception)
                {
                    LogError(pEndPoint + " --- " + exception.Message);
                }

                IsSending = false;
                if (pEnqueued)
                    CheckRequests();

                return responseObj;
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrlWithParameters, request.responseCode, request.error));

                IsSending = false;
                if (pEnqueued)
                    CheckRequests();

                return null;
            }
        }
        protected async Task<Q> SendJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, int pPort, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false) where Q : ResponseEmpty
        {
            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);

            if (NetworkManager.Instance.NetworkSetting.LogApiCall)
                LogAPI(pEndPoint);

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);

            string data = "";
            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrl);
                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);
                    break;


                case HttpMethod.Post:
                    request = new UnityWebRequest(completeUrl, "POST");

                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;


                case HttpMethod.Put:
                    request = new UnityWebRequest(completeUrl, "PUT");

                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;
            }
            ;

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOutMs;
            AddHeaders(request);

            string respose = "";
            Q responseObj = default;

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkManager.Instance.NetworkSetting.LogResponseTime)
                LogReponseTime(pEndPoint, Mathf.RoundToInt((pong - ping) * 1000));

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;
                if (NetworkManager.Instance.NetworkSetting.LogResponse)
                    LogResponse(respose, pEndPoint);

                try
                {
                    responseObj = JsonConvert.DeserializeObject<Q>(respose);
                }
                catch (Exception exception)
                {
                    LogError(pEndPoint + " --- " + exception.Message);
                }

                IsSending = false;
                if (pEnqueued)
                    CheckRequests();

                return responseObj;
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrl, request.responseCode, request.error));

                IsSending = false;
                if (pEnqueued)
                    CheckRequests();

                return null;
            }
        }
        protected async Task<string> SendSimpleRequest(string pEndPoint, RequestEmpty pRequest, int pPort, HttpMethod pMethod = HttpMethod.Post, bool pEnqueued = false)
        {
            IsSending = true;

            if (NetworkManager.Instance.NetworkSetting.Environment is NetworkManager.Environment.LocalHost && pPort != 0)
            {
                pEndPoint = string.Format("{0}/{1}", pPort, pEndPoint);
            }

            if (NetworkManager.Instance.NetworkSetting.LogApiCall)
                LogAPI(pEndPoint);

            string completeUrl = string.Concat(NetworkManager.Instance.NetworkSetting.Url, pEndPoint);

            string data = "";
            UnityWebRequest request = null;
            switch (pMethod)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(completeUrl);
                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);
                    break;


                case HttpMethod.Post:
                    request = new UnityWebRequest(completeUrl, "POST");

                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;


                case HttpMethod.Put:
                    request = new UnityWebRequest(completeUrl, "PUT");

                    data = JsonConvert.SerializeObject(pRequest);
                    if (NetworkManager.Instance.NetworkSetting.LogRequest)
                        LogRequest(data, pEndPoint);

                    SetJsonBody(request, data);
                    break;
            }
            ;

            request.timeout = NetworkManager.Instance.NetworkSetting.TimeOutMs;
            AddHeaders(request);

            string respose = "";

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkManager.Instance.NetworkSetting.LogResponseTime)
                LogReponseTime(pEndPoint, Mathf.RoundToInt((pong - ping) * 1000));

            IsSending = false;

            if (pEnqueued)
                CheckRequests();

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;

                if (NetworkManager.Instance.NetworkSetting.LogResponse)
                    LogResponse(respose, pEndPoint);
                return respose;
            }
            else
            {
                LogError(string.Format(format: "{0} ---> Code {1} --- {2}", completeUrl, request.responseCode, request.error));
                return null;
            }
        }

        protected virtual Task<Q> EnqueueJsonRequest<Q>(string pEndPoint, RequestEmpty pRequest, int pPort, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty
        {
            var tcs = new TaskCompletionSource<Q>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    Q result = await SendJsonRequest<Q>(pEndPoint, pRequest, pPort, pMethod, true);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckRequests();

            return tcs.Task;
        }
        protected virtual Task<Q> EnqueueUrlParamRequest<Q>(string pEndPoint, Dictionary<string, string> pParameter, string pBody, int pPort = 0, HttpMethod pMethod = HttpMethod.Post) where Q : ResponseEmpty
        {
            var tcs = new TaskCompletionSource<Q>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    Q result = await SendUrlParamsRequest<Q>(pEndPoint, pParameter, pBody, pPort, pMethod, true);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckRequests();

            return tcs.Task;
        }
        protected virtual Task<string> EnqueuSimpleRequest(string pEndPoint, RequestEmpty pRequest, int pPort, HttpMethod pMethod = HttpMethod.Post)
        {
            var tcs = new TaskCompletionSource<string>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    string result = await SendSimpleRequest(pEndPoint, pRequest, pPort, pMethod, true);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckRequests();

            return tcs.Task;
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
                string queryString = string.Join("&", pParameters.Select(p =>
                  Uri.EscapeDataString(p.Key) + "=" + Uri.EscapeDataString(p.Value)));

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
