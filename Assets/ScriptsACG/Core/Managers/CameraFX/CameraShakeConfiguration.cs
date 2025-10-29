using System;

namespace ToolsACG.Core.Models
{
    [Serializable]
    public class CameraShakeConfiguration
    {
        public float _shieldMinDistance = -1;
        public float _shieldMaxDistance = 1;
        public float _shieldShakeDuration = 0.3f;
        public float _shieldShakeMagnitude = 0.4f;
    }
}
