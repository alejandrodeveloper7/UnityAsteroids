using ToolsACG.Utils.Pooling;
using UnityEngine;

public class Sound3DController : MonoBehaviour, IPooleableGameObject
{
    #region Fields

    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private AudioSource _audioSource;

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Monobehviour

    private void Awake()
    {
        GetReferences();
    }

    #endregion

    #region Functionality

    public void Play(float pDelay)
    {
        if (pDelay > 0)
        {
            _audioSource.PlayDelayed(pDelay);
            Invoke(nameof(RecycleGameObject), _audioSource.clip.length + pDelay);
        }
        else
        {
            _audioSource.Play();
            Invoke(nameof(RecycleGameObject), _audioSource.clip.length);
        }
    }

    private void RecycleGameObject()
    {
        GetComponent<IPooleableGameObject>().Recycle(gameObject);
    }

    #endregion
}
