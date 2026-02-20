using ACG.Scripts.Models;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.Interfaces
{
    public interface IBulletData
    {
        SO_PooledGameObjectData PrefabData { get; }

        bool IsActive { get; }
        Sprite Sprite { get; }

        Color Color { get; }
        Vector3 Scale { get; }
        float BetweenBulletsTime { get; }

        float FullScaleDuration { get; }
        float DescaleDuration { get; }
        float LifeTime { get; }

        float PushForce { get; }
        float TorqueForce { get; }

        float Speed { get; }
        int Damage { get; }

        List<StatData> Stats { get; }

        List<ParticleSystemData> ParticlesOnDestruction { get; }

        ScreenEdgeTeleportData ScreenEdgeTeleportData { get; }

        List<SO_SoundData> SoundsOnShoot { get; }
    }
}
