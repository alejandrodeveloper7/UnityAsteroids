using System.Collections.Generic;
using System.Threading.Tasks;
using ToolsACG.Core.ScriptableObjects.Data;
using UnityEngine;

namespace ToolsACG.Core.Managers
{
    public interface ISoundManager
    {
        void SetMusicVolume(float newValue);
        void SetEffectsVolume(float newValue);

        void PlayMusicLoop();
        void StopMusicLoop(float progressivelyStopDuration);

        void Play2DSounds(List<SO_SoundData> data);
        Task Play2DSound(SO_SoundData data);

        void Play3DSounds(List<SO_SoundData> data, Vector3 position, Transform parent);
        Task Play3DSound(SO_SoundData data, Vector3 position, Transform parent);
    }
}