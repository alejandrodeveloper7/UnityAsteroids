using ACG.Core.Models;
using ACG.Scripts.Models;
using ACG.Scripts.ScriptableObjects.Data;
using ACG.Tools.Runtime.Pooling.ScriptableObjects;
using Asteroids.Core.Interfaces.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Core.Interfaces
{
    public interface IShipData
    {      
        SO_PooledGameObjectData PrefabData { get; }
        float TimeBeforeRecicle { get; }

        bool IsActive { get; }
        Sprite Sprite { get; }

        int HealthPoints { get; }
        float MovementSpeed { get; }
        float RotationSpeed { get; }
        float ShieldRecoveryTime { get; }

        List<StatData> Stats { get; }

        Color Color { get; }

        float PropulsionFireTransitionDuration { get; }
        Vector3 PropulsionFireLocalPosition { get; }

        Vector3 BulletsSpawnPointsLocalPosition { get; }

        int InvulnerabilityDuration { get; }
        int InvulnerabilityBlinksPerSecond { get; }
        int TotalInvulnerabilityBlinks { get; }
        float InvulnerabilityBlinkDuration { get; }

        Color ShieldColor { get; }

        float ShieldFadeInDuration { get; }
        float ShieldFadeOutDuration { get; }

        float ShieldBlinkDuration { get; }
        FloatRange ShieldBlinkAlphaRange { get; }

        FloatRange ShieldSliderValueRange { get; }

        Color ShieldSliderRecoveringColor { get; }
        Color ShieldSliderFullColor { get; }

        Color ShieldShineColor { get; }

        ScreenEdgeTeleportData ScreenEdgeTeleportData { get; }

        CameraShakeData ShieldLostCameraShakeData { get; }
        CameraShakeData DamageTakedCameraShakedata { get; }
        CameraShakeData DeadCameraShakeData { get; }

        List<ParticleSystemData> ParticlesOnDead { get; }

        List<SO_SoundData> SoundsOnShieldUp { get; }
        List<SO_SoundData> SoundsOnShieldDown { get; }

        List<SO_SoundData> SoundsOnDamage { get; }
        List<SO_SoundData> SoundsOnDestruction { get; }
    }
}