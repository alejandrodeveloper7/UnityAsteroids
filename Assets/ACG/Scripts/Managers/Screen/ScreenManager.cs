using ACG.Scripts.ScriptableObjects.Settings;
using ACG.Scripts.Services;
using ACG.Tools.Runtime.ManagersCreator.Bases;
using UnityEngine;
using Zenject;

namespace ACG.Scripts.Managers
{
    public class ScreenManager : NoMonobehaviourInstancesManagerBase, IScreenManager
    {
        #region Fields

        [Header("References")]
        private readonly IScreenService _screenService;

        [Header("Values")]
        private int _defaultSleepTimeout;

        [Header("Data")]
        private readonly SO_ScreenSettings _screenSettings;

        #endregion

        #region Constructors

        [Inject]
        public ScreenManager(IScreenService screenService, SO_ScreenSettings screenSettings)
        {
            _screenService = screenService;
            _screenSettings = screenSettings;

            Initialize();
        }

        #endregion

        #region Initialization

        public override void Initialize()
        {
            base.Initialize();

            _defaultSleepTimeout = _screenService.SleepTimeOut;

            if (_screenSettings.ApplyFrameRateLimit)
                ((IScreenManager)this).ApplyFrameRate();

#if UNITY_IOS || UNITY_ANDROID

            if (_screenSettings.ApplyInitialOrientation)
             ((IScreenManager)this).ApplyInitialOrientation();

            if (_screenSettings.ApplyRotationConfiguration)
             ((IScreenManager)this).ApplyOrientationConfiguration();

            if (_screenSettings.PreventSleep)
             ((IScreenManager)this).PreventSleep();
#endif
        }

        public override void Dispose()
        {
            base.Dispose();

#if UNITY_IOS || UNITY_ANDROID

            ((IScreenManager)this).RestoreSystemSleepTimeOut();          
#endif
        }

        #endregion

        // These methods are implemented explicitly so they don't show up as public members in IntelliSense.

        #region FrameRate

        void IScreenManager.ApplyFrameRate()
        {
            _screenService.SetTargetFrameRate(_screenSettings.FrameRate);
        }

        #endregion

        #region Orientation

        void IScreenManager.ApplyInitialOrientation()
        {
            _screenService.SetOrientation(_screenSettings.InitialOrientarion);
        }

        void IScreenManager.ApplyOrientationConfiguration()
        {
            _screenService.ConfigureAutoRotation(
                _screenSettings.AutoRotatePortrait,
                _screenSettings.AutoRotatePortraitUpsideDown,
                _screenSettings.AutoRotateLandscapeLeft,
                _screenSettings.AutoRotateLandscapeRight);
        }

        #endregion

        #region SleepTimeOut

        void IScreenManager.PreventSleep()
        {
            _screenService.PreventSleepTimeOut();
        }

        void IScreenManager.RestoreSystemSleepTimeOut()
        {
            _screenService.SetSleepTimeOut(_defaultSleepTimeout);
        }

        #endregion
    }
}