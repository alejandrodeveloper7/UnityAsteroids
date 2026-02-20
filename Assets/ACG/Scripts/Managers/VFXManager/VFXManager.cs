using ACG.Scripts.Models;
using ACG.Tools.Runtime.ManagersCreator.Bases;
using ACG.Tools.Runtime.Pooling.Core;
using ACG.Tools.Runtime.Pooling.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace ACG.Scripts.Managers
{
    public class VFXManager : NoMonobehaviourInstancesManagerBase, IVFXManager
    {
        #region Fields

        [Header("References")]
        private Transform _particlesGeneralParent;
        private Transform _detonablesGeneralParent;

        #endregion

        #region Constructors

        public VFXManager()
        {

        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            base.Initialize();

            CreateParticlesGeneralParent();
            CreateDetonablesGeneralParent();
        }

        public override void Dispose()
        {
            base.Dispose();

            DestroyParticlesGeneralParent();
            DestroyDetonablesGeneralParent();
        }

        #endregion

        #region Parents Management

        private void CreateParticlesGeneralParent()
        {
            _particlesGeneralParent = new GameObject("Particles_general_parent").transform;
            _particlesGeneralParent.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(_particlesGeneralParent.gameObject);
        }

        private void CreateDetonablesGeneralParent()
        {
            _detonablesGeneralParent = new GameObject("Detonables_general_parent").transform;
            _detonablesGeneralParent.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(_detonablesGeneralParent.gameObject);
        }

        private void DestroyParticlesGeneralParent()
        {
            GameObject.Destroy(_particlesGeneralParent);
        }

        private void DestroyDetonablesGeneralParent()
        {
            GameObject.Destroy(_detonablesGeneralParent);
        }

        #endregion

        #region Effects

        public void PlayParticlesVFX(List<ParticleSystemData> particlesDataList, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World)
        {
            if (particlesDataList is null || particlesDataList.Count == 0)
                return;

            foreach (ParticleSystemData particle in particlesDataList)
                PlayParticleVFX(particle, position, rotation, parent, space);
        }
        public void PlayParticleVFX(ParticleSystemData particlesData, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World)
        {
            if (particlesData is null)
                return;

            GameObject vfx = FactoryManager.Instance.GetGameObjectInstance(particlesData.PrefabData);

            if (particlesData.ParticleConfiguration != null)
                particlesData.ParticleConfiguration.ApplyConfiguration(vfx.GetComponent<ParticleSystem>());

            if (parent == null)
                parent = _particlesGeneralParent;

            vfx.transform.SetParent(parent, false);

            if (rotation == default)
                rotation = Quaternion.identity;

            Vector3 finalPos;
            Quaternion finalRot = rotation * Quaternion.Euler(particlesData.RotationOffset);

            if (space == Space.World)
            {
                finalPos = position + rotation * particlesData.PositionOffset;
                vfx.transform.SetPositionAndRotation(finalPos, finalRot);
            }
            else
            {
                finalPos = position + particlesData.PositionOffset;
                vfx.transform.SetLocalPositionAndRotation(finalPos, finalRot);
            }

            vfx.GetComponent<IPooledDetonable>().Detonate();
        }
             

        public void PlayDetonables(List<DetonableData> DetonablesDataList, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World)
        {
            if (DetonablesDataList is null || DetonablesDataList.Count == 0)
                return;

            foreach (DetonableData detonable in DetonablesDataList)
                PlayDetonable(detonable, position, rotation, parent, space);
        }
        public void PlayDetonable(DetonableData detonableData, Vector3 position = default, Quaternion rotation = default, Transform parent = null, Space space = Space.World)
        {
            if (detonableData is null)
                return;

            GameObject detonable = FactoryManager.Instance.GetGameObjectInstance(detonableData.PrefabData);

            if (parent == null)
                parent = _detonablesGeneralParent;

            detonable.transform.SetParent(parent, false);

            if (rotation == default)
                rotation = Quaternion.identity;

            Vector3 finalPos;
            Quaternion finalRot = rotation * Quaternion.Euler(detonableData.RotationOffset);

            if (space == Space.World)
            {
                finalPos = position + rotation * detonableData.PositionOffset;
                detonable.transform.SetPositionAndRotation(finalPos, finalRot);
            }
            else
            {
                finalPos = position + detonableData.PositionOffset;
                detonable.transform.SetLocalPositionAndRotation(finalPos, finalRot);
            }

            detonable.GetComponent<IPooledDetonable>().Detonate();
        }

        #endregion
    }
}
