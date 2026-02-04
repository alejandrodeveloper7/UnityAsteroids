using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACG.Scripts.Services
{
    public interface IScreenService
    {
        //Zenject AsSingle() requiered for events

        event Action<Vector2Int> OnResolutionChanged;
        event Action<FullScreenMode> OnFullScreenModeChanged;

        List<Vector2Int> AvailableResolutions { get; }
        List<string> AvailableResolutionsOptions { get; }

        Vector2Int CurrentResolution { get; }
        bool IsFullScreen { get; }
        int SleepTimeOut { get; }


        void SetTargetFrameRate(int value);

        void ToggleFullScreen();

        void SetFullScreenMode(bool active);

        void SetResolution(int index);
        void SetResolution(Vector2Int resolution);
        void RefreshAvailableResolutions();

        void SetOrientation(ScreenOrientation orientation);
        void ConfigureAutoRotation(bool portrait, bool portraitUpsideDown, bool landscapeLeft, bool landscapeRight);

        void PreventSleepTimeOut();
        void SetSleepTimeOut(int sleepTimeOut);
    }
}