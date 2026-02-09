using System;

namespace ACG.Scripts.Models
{
    [Serializable]
    public class ScreenEdgeTeleportData
    {
        public float EdgeOffsetY = 0.05f;
        public float EdgeRepositionOffsetY = 0.04f;

        public float EdgeOffsetX = 0.04f;
        public float EdgeRepositionOffsetX = 0.03f;

        public float CheckInterval = 0.05f;
    }
}