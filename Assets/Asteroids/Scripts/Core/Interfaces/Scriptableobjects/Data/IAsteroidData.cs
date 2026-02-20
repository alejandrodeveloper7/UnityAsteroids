using ACG.Core.Models;
using ACG.Scripts.Models;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using Asteroids.Core.ScriptableObjects.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.Interfaces
{
    public interface IAsteroidData
    {
        SO_PooledGameObjectData PrefabData { get; }

        bool IsInitialAsteroid { get; }
        List<Sprite> PossibleSpritesList { get; }
        Color Color { get; }

        Vector3 Scale { get; }
        float AsteroidSpawnAngle { get; }
        FloatRange TorqueRange { get; }
        float AlphaAparitionDuration { get; }

        int MaxHP { get; }

        float Speed { get; }
        int ScoreValue { get; }
        int Damage { get; }

        Color DamageFeedbackColor { get; }
        Vector3 DamageFeedbackScale { get; }
        float DamageFeedbackDuration { get; }

        int FragmentsAmountGeneratedOnDestruction { get; }
        List<SO_AsteroidData> FragmentTypesGeneratedOnDestruction { get; }

        ScreenEdgeTeleportData ScreenEdgeTeleportData { get; }

        List<ParticleSystemData> ParticlesOnDamage { get; }
        List<ParticleSystemData> ParticlesOnDestruction { get; }

        List<SO_SoundData> SoundsOnDamage { get; }
        List<SO_SoundData> SoundsOnDestruction { get; }
    }
}