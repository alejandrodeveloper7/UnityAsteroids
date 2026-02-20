using ACG.Core.Utils;
using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using UnityEngine;

namespace ACG.Scripts.Services
{
    public class DebugService : InstancesServiceBase, IDebugService
    {
        #region Fields

        [Header("Data")]
        private readonly SO_DebugSettings _debugSettings;

        #endregion

        #region Constructors

        public DebugService(SO_DebugSettings debugSettings)
        {
            if (debugSettings == null)
                debugSettings = SO_DebugSettings.Instance;
            
            _debugSettings = debugSettings;
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method for initial logic and event subscriptions (called by Zenject)
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: Clean here all the listeners or elements that need be clean when the script is destroyed (called by Zenject)
        }

        #endregion      

        #region Logs

        public void Log(string topic, string message, Color? topicColor = null, Color? color = null)
        {
            topicColor ??= Color.white;
            color ??= Color.white;

            string topicText = $"<color=#{ColorUtils.GetHexadecimalFromColor(topicColor.Value)}>{_debugSettings.TopicIndicators} {topic} {_debugSettings.TopicIndicators}</color>";
            string messageText = $"<color=#{ColorUtils.GetHexadecimalFromColor(color.Value)}>{message}</color>";

            Debug.Log($"{topicText} {messageText}");
        }

        public void Warning(string topic, string message, Color? topicColor = null, Color? color = null)
        {
            topicColor ??= Color.white;
            color ??= Color.white;

            string warningColorHex = ColorUtils.GetHexadecimalFromColor(_debugSettings.WarningMessageColor);

            string topicText = $"<color=#{ColorUtils.GetHexadecimalFromColor(topicColor.Value)}>{_debugSettings.TopicIndicators} {topic} {_debugSettings.TopicIndicators}</color>";
            string messageText = $"<color=#{ColorUtils.GetHexadecimalFromColor(color.Value)}>{message}</color>";
            string warningTagText = $"<color=#{warningColorHex}>{_debugSettings.WarningMessage}</color>";

            Debug.LogWarning($"{topicText} {messageText} {warningTagText}");
        }

        public void Error(string topic, string message, Color? topicColor = null, Color? color = null)
        {
            topicColor ??= Color.white;
            color ??= Color.white;

            string errorColorHex = ColorUtils.GetHexadecimalFromColor(_debugSettings.ErrorMessageColor);

            string topicText = $"<color=#{ColorUtils.GetHexadecimalFromColor(topicColor.Value)}>{_debugSettings.TopicIndicators} {topic} {_debugSettings.TopicIndicators}</color>";
            string messageText = $"<color=#{ColorUtils.GetHexadecimalFromColor(color.Value)}>{message}</color>";
            string errorTagText = $"<color=#{errorColorHex}>{_debugSettings.ErrorMessage}</color>";

            Debug.LogError($"{topicText} {messageText} {errorTagText}");
        }

        #endregion

        #region Configuration

        public void SetLogActive(bool state)
        {
            Debug.unityLogger.logEnabled = state;
            Debug.Log($"- Debug - Debug log state set to {state}.");
        }

        #endregion
    }
}
