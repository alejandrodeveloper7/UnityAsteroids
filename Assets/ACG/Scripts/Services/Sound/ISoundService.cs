namespace ACG.Scripts.Services
{
    public interface ISoundService
    {
        void SetMusicVolume(float newValue);
        void SetMuteMusicState(bool state);

        void SetEffectsVolume(float newValue);
        void SetMuteEffectsState(bool state);
    }
}