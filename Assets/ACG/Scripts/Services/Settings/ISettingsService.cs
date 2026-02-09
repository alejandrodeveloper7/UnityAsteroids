using System.Collections.Generic;

namespace ACG.Scripts.Services
{
   public interface ISettingsService 
   {
        void ApplyAllStoredSettings();

        void SetStoredFullScreen();
        void SetFullScreen(bool isFullScreen);
        bool GetFullScreen();

        void SetStoredMusicVolume();
        void SetMusicVolume(float value);
        void SaveMusicVolume(float value);
        float GetMusicVolume();

        void SetStoredMusicMute();
        void SetMusicMute(bool state);
        bool GetMusicMuted();

        void SetStoredEffectsVolume();
        void SetEffectsVolume(float value);
        void SaveEffectsVolume(float value);
        float GetEffectsVolume();

        void SetStoredEffectsMute();
        void SetEffectsMute(bool state);
        bool GetEffectsMuted();

        void SetStoredResolution();
        void SetResolution(int index);
        int GetResolutionIndex();
        List<string> GetAvailableResolutionOptions();
    }
}