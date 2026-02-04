using System;
using System.Collections.Generic;
using System.Linq;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using UnityEngine;

namespace ACG.Scripts.Services
{
    public class ScreenService : InstancesServiceBase, IScreenService
    {
        #region Fields, properties and events

        public event Action<Vector2Int> OnResolutionChanged;
        public event Action<FullScreenMode> OnFullScreenModeChanged;

        [Header("Values")]
        private List<Vector2Int> _cachedResolutions;
        public List<Vector2Int> AvailableResolutions
        {
            get
            {
                _cachedResolutions ??= Screen.resolutions
                    .Select(res => new Vector2Int(res.width, res.height))
                    .Distinct()
                    .ToList();

                return _cachedResolutions;
            }
        }

        private List<string> _cachedAvailableResolutionsOptions;
        public List<string> AvailableResolutionsOptions
        {
            get
            {
                if (_cachedAvailableResolutionsOptions == null)
                {
                    _cachedAvailableResolutionsOptions = new List<string>();

                    foreach (Vector2Int resolution in AvailableResolutions)
                        _cachedAvailableResolutionsOptions.Add($"{resolution.x} X {resolution.y}");
                }

                return _cachedAvailableResolutionsOptions;
            }
        }

        public Vector2Int CurrentResolution => new(Screen.currentResolution.width, Screen.currentResolution.height);
        public bool IsFullScreen => Screen.fullScreenMode != FullScreenMode.Windowed;
        public int SleepTimeOut => Screen.sleepTimeout;

        #endregion

        #region Constructors

        public ScreenService()
        {
            Initialize();
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion       

        #region FrameRate

        public void SetTargetFrameRate(int value)
        {
            Application.targetFrameRate = value;
            Debug.Log($"- {typeof(ScreenService).Name} - FrameRate Fixed: " + value);
        }

        #endregion

        #region FullScreen

        public void ToggleFullScreen()
        {
            bool isFull = Screen.fullScreenMode is not FullScreenMode.Windowed;
            SetFullScreenMode(!isFull);
        }

        public void SetFullScreenMode(bool active)
        {
            if (active)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;

            OnFullScreenModeChanged?.Invoke(Screen.fullScreenMode);
            Debug.Log($"- {typeof(ScreenService).Name} - Full screen mode is {Screen.fullScreenMode}");
        }

        #endregion

        #region Resolution

        public void SetResolution(int index)
        {
            if (index < 0 || index >= AvailableResolutions.Count)
            {
                Debug.LogWarning("Invalid resolution index");
                return;
            }

            Vector2Int resolution = AvailableResolutions[index];
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
            Debug.Log($"- {typeof(ScreenService).Name} - Resolution is {resolution.x}x{resolution.y}");

            OnResolutionChanged?.Invoke(resolution);
        }

        public void SetResolution(Vector2Int resolution)
        {
            Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
            Debug.Log($"- {typeof(ScreenService).Name} - Resolution is {resolution.x}x{resolution.y}");

            OnResolutionChanged?.Invoke(resolution);
        }

        public void RefreshAvailableResolutions()
        {
            _cachedResolutions = Screen.resolutions
                            .Select(res => new Vector2Int(res.width, res.height))
                            .Distinct()
                            .ToList();

            _cachedAvailableResolutionsOptions = new List<string>();

            foreach (Vector2Int resolution in AvailableResolutions)
                _cachedAvailableResolutionsOptions.Add($"{resolution.x}x{resolution.y}");
        }

        #endregion

        #region Orientation

        public void SetOrientation(ScreenOrientation orientation)
        {
            Screen.orientation = orientation;
            Debug.Log($"- {typeof(ScreenService).Name} - Screen orientation set to {orientation}");
        }

        public void ConfigureAutoRotation(bool portrait, bool portraitUpsideDown, bool landscapeLeft, bool landscapeRight)
        {
            Screen.autorotateToPortrait = portrait;
            Screen.autorotateToPortraitUpsideDown = portraitUpsideDown;
            Screen.autorotateToLandscapeLeft = landscapeLeft;
            Screen.autorotateToLandscapeRight = landscapeRight;

            Debug.Log($"- {typeof(ScreenService).Name} - AutoRotation configured: Portrait={portrait}, PortraitUpsideDown={portraitUpsideDown}, LandscapeLeft={landscapeLeft}, LandscapeRight={landscapeRight}");
        }

        #endregion

        #region Sleep

        public void PreventSleepTimeOut()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Debug.Log($"- {typeof(ScreenService).Name} - Sleep mode set to NeverSleep.");
        }

        public void SetSleepTimeOut(int sleepTimeOut)
        {
            Screen.sleepTimeout = sleepTimeOut;
            Debug.Log($"- {typeof(ScreenService).Name} - Sleep mode set to {sleepTimeOut}.");
        }

        #endregion
    }
}
