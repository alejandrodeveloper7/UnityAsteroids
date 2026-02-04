using UnityEngine;

namespace ACG.Scripts.Services
{
    public interface IDebugService
    {
        void Log(string topic, string message, Color? topicColor = null, Color? color = null);
        void Warning(string topic, string message, Color? topicColor = null, Color? color = null);
        void Error(string topic, string message, Color? topicColor = null, Color? color = null);
        void SetLogActive(bool state);
    }
}
