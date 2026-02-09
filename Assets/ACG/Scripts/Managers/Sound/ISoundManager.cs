using System.Collections.Generic;
using System.Threading.Tasks;
using ACG.Scripts.ScriptableObjects.Data;
using UnityEngine;

namespace ACG.Scripts.Managers
{
    public interface ISoundManager
    {      
        void PlayMusicLoop();
        void StopMusicLoop(float progressivelyStopDuration);

        void Play2DSounds(List<SO_SoundData> data);
        Task Play2DSound(SO_SoundData data);

        void Play3DSounds(List<SO_SoundData> data, Vector3 position, Transform parent = null);
        Task Play3DSound(SO_SoundData data, Vector3 position, Transform parent = null);
    }
}