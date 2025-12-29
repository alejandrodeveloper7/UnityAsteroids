using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.ManagersCreator.Bases;
using ToolsACG.Pooling.Core;
using ToolsACG.Pooling.Gameplay;
using ToolsACG.Pooling.Interfaces;
using ToolsACG.Pooling.ScriptableObjects;
using UnityEngine;

namespace ToolsACG.Core.Managers
{
    public class VFXManager : NoMonobehaviourInstancesManagerBase, IVFXManager
    {
        #region Fields

        [Header("References")]
        private Transform _particlesGeneralParent;

        #endregion

        #region Constructors

        public VFXManager()
        {
            Initialize();
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            base.Initialize();

            CreateParticlesGeneralParent();
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the listeners or elements that need be clean when the manager is destroyed
        }

        #endregion

        #region Parents Creation

        private void CreateParticlesGeneralParent()
        {
            _particlesGeneralParent = new GameObject("particles_general_parent").transform;
            _particlesGeneralParent.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(_particlesGeneralParent.gameObject);
        }

        #endregion


        #region Functionality

        public void PlayParticlesVFX(SO_PooledGameObjectData pooledGameObjectData, Vector3 position, Quaternion rotation, Transform parent = null, SO_ParticleConfigurationBase particlesConfig = null)
        {
            GameObject vfx = FactoryManager.Instance.GetGameObjectInstance(pooledGameObjectData);

            if (particlesConfig != null)
                particlesConfig.ApplyConfiguration(vfx.GetComponent<ParticleSystem>());

            if (parent == null)
            {
                parent = _particlesGeneralParent;
                vfx.transform.SetParent(parent);
                vfx.transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                vfx.transform.SetParent(parent);
                vfx.transform.SetPositionAndRotation(position, rotation);
            }

            vfx.GetComponent<IPooledDetonable>().Play();
        }

        #endregion
    }
}
