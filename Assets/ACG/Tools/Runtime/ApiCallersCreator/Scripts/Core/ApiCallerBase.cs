using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACG.Tools.Runtime.ApiCallersCreator.Enums;
using ACG.Tools.Runtime.ApiCallersCreator.Models;
using ACG.Tools.Runtime.ApiCallersCreator.ScriptableObjects;
using UnityEngine;
using UnityEngine.Networking;

namespace ACG.Tools.Runtime.ApiCallersCreator.Core
{
    public class ApiCallerBase<T> where T : class, new()
    {
        #region Fields and Properties

        [Header("Singleton")]
        private static T _instance;
        public static T Instance
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }

        [Header("References")]

        protected Queue<Action> ExecutionQueue = new();
        [Header("States")]
        protected bool IsSending;

        [Header("Data")]
        protected SO_NetworkSettings NetworkSettings;

        #endregion

        #region Cosntructor

        public ApiCallerBase()
        {
            NetworkSettings = SO_NetworkSettings.Instance;
        }

        #endregion

        #region Apis management

        protected async Task<Q> SendUrlParamsRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, string body, HttpMethod method = HttpMethod.Post, bool enqueued = false, string authToken = null) where Q : ResponseEmpty
        {
            IsSending = true;

            if (NetworkSettings.LogApiCall)
                LogAPI(endPoint);

            UnityWebRequest request = null;
            switch (method)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(endPoint);
                    if (NetworkSettings.LogRequest)
                        LogRequest(body, endPoint);
                    break;


                case HttpMethod.Post:
                case HttpMethod.Put:
                    if (string.IsNullOrEmpty(body))
                    {
                        request = UnityWebRequest.PostWwwForm(endPoint, "");
                        if (NetworkSettings.LogRequest)
                            LogRequest(body, endPoint);
                    }
                    else
                    {
                        request = UnityWebRequest.PostWwwForm(endPoint, body);
                        SetJsonBody(request, body);
                        if (NetworkSettings.LogRequest)
                            LogRequest(body, endPoint);
                    }
                    break;
            }
            ;

            request.timeout = configuration.TimeOutMs;
            ApplyHeaders(request,configuration, authToken);

            string respose;
            Q responseObj = default;

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkSettings.LogResponseTime)
                LogReponseTime(endPoint, Mathf.RoundToInt((pong - ping) * 1000));


            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;
                if (NetworkSettings.LogResponse)
                    LogResponse(respose, endPoint);

                try
                {
                    responseObj = JsonConvert.DeserializeObject<Q>(respose);
                }
                catch (Exception exception)
                {
                    LogError($"{endPoint} --- {exception.Message}");
                }

                IsSending = false;
                if (enqueued)
                    CheckEnqueueRequests();

                return responseObj;
            }
            else
            {
                LogError($"{endPoint} ---> Code {request.responseCode} --- {request.error}");

                IsSending = false;
                if (enqueued)
                    CheckEnqueueRequests();

                return null;
            }
        }
        protected async Task<Q> SendJsonRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty requestData, HttpMethod method = HttpMethod.Post, bool enqueued = false, string authToken = null) where Q : ResponseEmpty
        {
            IsSending = true;

            if (NetworkSettings.LogApiCall)
                LogAPI(endPoint);

            string data;
            UnityWebRequest request = null;
            switch (method)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(endPoint);
                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);
                    break;


                case HttpMethod.Post:
                    request = new UnityWebRequest(endPoint, "POST");

                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);

                    SetJsonBody(request, data);
                    break;


                case HttpMethod.Put:
                    request = new UnityWebRequest(endPoint, "PUT");

                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);

                    SetJsonBody(request, data);
                    break;
            }
            ;

            request.timeout = configuration.TimeOutMs;
            ApplyHeaders(request, configuration, authToken);

            string respose;
            Q responseObj = default;

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkSettings.LogResponseTime)
                LogReponseTime(endPoint, Mathf.RoundToInt((pong - ping) * 1000));

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;
                if (NetworkSettings.LogResponse)
                    LogResponse(respose, endPoint);

                try
                {
                    responseObj = JsonConvert.DeserializeObject<Q>(respose);
                }
                catch (Exception exception)
                {
                    LogError($"{endPoint} --- {exception.Message}");
                }

                IsSending = false;
                if (enqueued)
                    CheckEnqueueRequests();

                return responseObj;
            }
            else
            {
                LogError($"{endPoint} ---> Code {request.responseCode} --- {request.error}");

                IsSending = false;
                if (enqueued)
                    CheckEnqueueRequests();

                return null;
            }
        }
        protected async Task<string> SendSimpleRequest(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty requestData, HttpMethod method = HttpMethod.Post, bool enqueued = false, string authToken = null)
        {
            IsSending = true;

            if (NetworkSettings.LogApiCall)
                LogAPI(endPoint);

            string data;
            UnityWebRequest request = null;
            switch (method)
            {
                case HttpMethod.Get:
                    request = UnityWebRequest.Get(endPoint);
                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);
                    break;


                case HttpMethod.Post:
                    request = new UnityWebRequest(endPoint, "POST");

                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);

                    SetJsonBody(request, data);
                    break;


                case HttpMethod.Put:
                    request = new UnityWebRequest(endPoint, "PUT");

                    data = JsonConvert.SerializeObject(requestData);
                    if (NetworkSettings.LogRequest)
                        LogRequest(data, endPoint);

                    SetJsonBody(request, data);
                    break;
            }
            ;

            request.timeout = configuration.TimeOutMs;
            ApplyHeaders(request, configuration, authToken);

            string respose;

            float ping = Time.time;

            _ = request.SendWebRequest();

            while (request.result is UnityWebRequest.Result.InProgress)
                await Task.Yield();

            float pong = Time.time;
            if (NetworkSettings.LogResponseTime)
                LogReponseTime(endPoint, Mathf.RoundToInt((pong - ping) * 1000));

            IsSending = false;

            if (enqueued)
                CheckEnqueueRequests();

            if (request.result is UnityWebRequest.Result.Success)
            {
                respose = request.downloadHandler.text;

                if (NetworkSettings.LogResponse)
                    LogResponse(respose, endPoint);
                return respose;
            }
            else
            {
                LogError($"{endPoint} ---> Code {request.responseCode} --- {request.error}");
                return null;
            }
        }

        protected virtual Task<Q> EnqueueJsonRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty requestData, HttpMethod method = HttpMethod.Post, string authToken = null) where Q : ResponseEmpty
        {
            var tcs = new TaskCompletionSource<Q>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    Q result = await SendJsonRequest<Q>(configuration, endPoint, requestData, method, true, authToken);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckEnqueueRequests();

            return tcs.Task;
        }
        protected virtual Task<Q> EnqueueUrlParamRequest<Q>(SO_NetworkConfiguration configuration, string endPoint, string body, HttpMethod method = HttpMethod.Post, string authToken = null) where Q : ResponseEmpty
        {
            var tcs = new TaskCompletionSource<Q>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    Q result = await SendUrlParamsRequest<Q>(configuration, endPoint, body, method, true, authToken);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckEnqueueRequests();

            return tcs.Task;
        }
        protected virtual Task<string> EnqueuSimpleRequest(SO_NetworkConfiguration configuration, string endPoint, RequestEmpty requestData, HttpMethod method = HttpMethod.Post, string authToken = null)
        {
            var tcs = new TaskCompletionSource<string>();

            ExecutionQueue.Enqueue(async () =>
            {
                try
                {
                    string result = await SendSimpleRequest(configuration, endPoint, requestData, method, true, authToken);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            if (IsSending is false)
                CheckEnqueueRequests();

            return tcs.Task;
        }

        private void CheckEnqueueRequests()
        {
            if (ExecutionQueue.Count > 0)
                ExecutionQueue.Dequeue().Invoke();
        }
        protected virtual void CleanQueue()
        {
            ExecutionQueue.Clear();
            IsSending = false;
            Debug.Log($"---------------  {GetType().Name} QUEUE CLEANED  ---------------");
        }

        #endregion

        #region Utility

        protected string BuildUrl(SO_NetworkConfiguration config, string endpoint, int port, Dictionary<string, string> parameters = null)
        {
            string baseUrl = config.Url.TrimEnd('/');
            string finalEndpoint = endpoint.TrimStart('/');

            string url = NetworkSettings.Environment is ServerEnvironment.LocalHost
                ? $"{baseUrl}:{port}/{finalEndpoint}"
                : $"{baseUrl}/{finalEndpoint}";

            if (parameters != null && parameters.Count > 0)
                BuildUrlParams(url, parameters);

            return url;
        }

        protected string BuildUrlParams(string url, Dictionary<string, string> parameters)
        {
            if (parameters is not null && parameters.Count > 0)
            {
                string queryString = string.Join("&", parameters.Select(p =>
                  Uri.EscapeDataString(p.Key) + "=" + Uri.EscapeDataString(p.Value)));

                url += (url.Contains("?") ? "&" : "?") + queryString;
            }
            return url;
        }

        protected void SetJsonBody(UnityWebRequest request, string jsonBody)
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
        }

        private void ApplyHeaders(UnityWebRequest request, SO_NetworkConfiguration configuration, string authToken = null)
        {
            if (!string.IsNullOrEmpty(configuration.ApiKey))
                request.SetRequestHeader("x-api-key", configuration.ApiKey);

            if (configuration.RequiresAuthToken && !string.IsNullOrEmpty(authToken))
                request.SetRequestHeader("Authorization", "Bearer " + authToken);

            foreach (var header in configuration.CustomHeaders)
                request.SetRequestHeader(header.Key, header.Value);
        }

        #endregion

        #region Log

        protected void LogAPI(string url)
        {
            Debug.LogFormat("<color=yellow>API ---> {0}</color>", url);
        }
        protected void LogRequest(object request, string url)
        {
            Debug.LogFormat("<color=Cyan>REQUEST --- {0} ---></color> {1}", url, request);
        }
        protected void LogResponse(object response, string url)
        {
            Debug.LogFormat("<color=lime>RESPONSE --- {0} ---></color> {1}", url, response);
        }
        protected void LogError(string text)
        {
            Debug.LogErrorFormat("<color=red>[ERROR] --- {0} --- </color>", text);
        }
        protected void LogReponseTime(string url, float time)
        {
            Debug.LogFormat("<color=orange>TIME RESPONE {0} ms --- {1}</color>", time, url);
        }

        #endregion
    }
}
