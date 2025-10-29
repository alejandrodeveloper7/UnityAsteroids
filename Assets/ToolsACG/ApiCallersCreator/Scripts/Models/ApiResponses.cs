using System;

namespace ToolsACG.ApiCallersCreator.Models
{
    [Serializable]
    public class ResponseEmpty
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
}