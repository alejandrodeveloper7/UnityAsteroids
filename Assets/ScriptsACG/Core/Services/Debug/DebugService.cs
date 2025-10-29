using ToolsACG.Core.ScriptableObjects.Settings;
using ToolsACG.Core.Utils;
using ToolsACG.ServicesCreator.Bases;
using UnityEngine;
using Zenject;

namespace ToolsACG.Core.Services
{
    public class DebugService : InstancesServiceBase, IDebugService
    {
        #region Fields

        [Header("Data")]
        private readonly SO_DebugSettings _debugSettings;

        #endregion

        #region Constructors

        [Inject]
        public DebugService(SO_DebugSettings debugSettings)
        {
            _debugSettings = debugSettings;

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
