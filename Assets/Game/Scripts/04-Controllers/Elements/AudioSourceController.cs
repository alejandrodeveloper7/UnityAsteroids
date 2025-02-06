using ToolsACG.Utils.Events;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class AudioSourceController : MonoBehaviour, IPooleableItem
{
    #region Fields

    [Header("IPooleableItem")]
    SimplePool _originPool;
    public SimplePool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("References")]
    private AudioSource _audioSource;
    
    #endregion

    #region Monobehviour

    private void Awake()
    {
        GetReferences();
    }

    private void OnEnable()
    {
        EventManager.GetGameplayBus().AddListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GetUiBus().AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GetGameplayBus().RemoveListener<PauseStateChanged>(OnPauseStateChanged);
        EventManager.GetUiBus().RemoveListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus callbacks

    private void OnPauseStateChanged(PauseStateChanged pPauseStateChanged)
    {
        if (pPauseStateChanged.InPause)
            _audioSource.Pause();
        else
            _audioSource.UnPause();
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        CancelInvoke(nameof(RecycleGameObject));
        _audioSource.Stop();
        RecycleGameObject();
    }

    #endregion

    #region Initialization
    
    private void GetReferences()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Functionality

    public void Play()
    {
        _audioSource.Play();
        Invoke(nameof(RecycleGameObject), _audioSource.clip.length);
    }

    private void RecycleGameObject()
    {
        OriginPool.RecycleItem(gameObject);
    }

    #endregion

}
