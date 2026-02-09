using System;

namespace ACG.Scripts.Models
{
    [Serializable]
    public class CameraShakeData
    {
        public float _shieldMinDistance = -1;
        public float _shieldMaxDistance = 1;
        public float _shieldShakeDuration = 0.3f;
        public float _shieldShakeMagnitude = 0.4f;
    }
}
