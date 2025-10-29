using ToolsACG.Core.ScriptableObjects.ParticleSystemConfigs;
using ToolsACG.ManagersCreator.Bases;
using ToolsACG.Pooling.Core;
using ToolsACG.Pooling.Gameplay;
using UnityEngine;

namespace ToolsACG.Core.Managers
{
    public class VFXManager : NoMonobehaviourInstancesManagerBase, IVFXManager
    {
        #region Fields

        [Header("References")]
        private Transform _vfxGeneralParent;

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

            CreateGeneralParent();
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the listeners or elements that need be clean when the manager is destroyed
        }

        #endregion

        #region Functionality

        private void CreateGeneralParent()
        {
            _vfxGeneralParent = new GameObject("VFX_general_parent").transform;
            _vfxGeneralParent.transform.position = Vector3.zero;
            GameObject.DontDestroyOnLoad(_vfxGeneralParent.gameObject);
        }

        public void PlayParticlesVFX(string name, Vector3 position, Transform parent = null, SO_ParticleConfigurationBase particlesConfig = null)
        {
            if (parent == null)
                parent = _vfxGeneralParent;

            GameObject vfx = FactoryManager.Instance.GetGameObjectInstance(name);

            if (particlesConfig != null)
                particlesConfig.ApplyConfiguration(vfx.GetComponent<ParticleSystem>());

            vfx.transform.SetParent(parent);
            vfx.transform.localPosition = position;
            vfx.GetComponent<PooledParticleSystem>().Play();
        }

        #endregion
    }
}
