using ToolsACG.Utils.Pooling;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour, IPooleableGameObject
{
    #region Fields

    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private ParticleSystem _particleSystem;

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
    }

    #endregion

    #region Functionality

    public void Play(float pDelay)
    {
        if (pDelay == 0)
            PlayEffect();
        else
            Invoke(nameof(PlayEffect), pDelay);
    }

    private void PlayEffect()
    {
        float duration = _particleSystem.main.duration;
        _particleSystem.Play();
        Invoke(nameof(RecycleGameObject), duration);
    }

    private void RecycleGameObject()
    {
        GetComponent<IPooleableGameObject>().Recycle(gameObject);
    }

    #endregion
}
