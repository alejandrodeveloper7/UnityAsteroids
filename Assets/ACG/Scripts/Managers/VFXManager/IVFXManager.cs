using ACG.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

namespace ACG.Scripts.Managers
{
    public interface IVFXManager
    {
        void PlayParticleVFX(ParticleSystemData particlesData, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World);
        void PlayParticlesVFX(List<ParticleSystemData> particlesDataList, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World);

        void PlayDetonable(DetonableData detonableData, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World);
        void PlayDetonables(List<DetonableData> detonablesData, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World);
    }
}