using Asteroids.Core.ScriptableObjects.Data;

namespace Asteroids.Core.Services
{
    public class RuntimeDataContainer
    {
        public SO_BulletData SelectedBulletData { get; set; }
        public SO_ShipData SelectedShipData { get; set; }
        public string UserName { get; set; }
        public int LastScore { get; set; }
    }
}
