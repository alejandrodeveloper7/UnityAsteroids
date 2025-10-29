using System.Collections.Generic;

namespace Asteroids.MVC.PauseUI.Views
{
    public interface IPauseUIView
    {
        void SetMusicSliderValue(float value);
        void SetEffectsSliderValue(float value);
        void SetFullScreenModeToggleState(bool state);
        void SetResolutionsDropdownOptionsAndIndex(List<string> options, int selectedIndex);
    }
}