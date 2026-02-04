namespace ACG.Scripts.Services
{
   public interface ISettingsService 
   {
        void ApplyAllStoredSettings();

        void ApplyStoredFullScreen();
        void SetFullScreen(bool isFullScreen);
        bool GetFullScreen();

        //Set and save are separated in the volume methods for better performance when are configured using sliders

        void ApplyStoredMusicVolume();
        void SetMusicVolume(float value);
        void SaveMusicVolume(float value);
        float GetMusicVolume();

        void ApplyStoredEffectsVolume();
        void SetEffectsVolume(float value);
        void SaveEffectsVolume(float value);
        float GetEffectsVolume();

        void ApplyStoredResolution();
        void SetResolution(int index);
        int GetResolutionIndex();
    }
}