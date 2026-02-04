using Asteroids.Core.ScriptableObjects.Data;
using System;

namespace Asteroids.Core.Services
{
    public class RuntimeDataContainer
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }

        public SO_BulletData SelectedBulletData { get; set; }
        public SO_ShipData SelectedShipData { get; set; }
        public string UserName { get; set; }
        public int LastScore { get; set; }
    }
}
