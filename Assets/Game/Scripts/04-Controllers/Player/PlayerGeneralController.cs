using System.Threading.Tasks;
using ToolsACG.Utils.Pooling;
using UnityEngine;

public class PlayerGeneralController : MonoBehaviour, IPooleableGameObject
{
    [Header("IPooleableItem")]
    SimpleGameObjectPool _originPool;
    public SimpleGameObjectPool OriginPool { get { return _originPool; } set { _originPool = value; } }
    bool _readyToUse;
    public bool ReadyToUse { get { return _readyToUse; } set { _readyToUse = value; } }

    [Header("Data")]
    private PlayerSettings _playerSettings;

    #region Monobehavior

    private void Awake()
    {
        _playerSettings = ResourcesManager.Instance.GetScriptableObject<PlayerSettings>(ScriptableObjectKeys.PLAYER_SETTINGS_KEY);
    }

    private void OnEnable()
    {
        EventManager.GameplayBus.AddListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    private void OnDisable()
    {
        EventManager.GameplayBus.RemoveListener<PlayerDead>(OnPlayerDead);
        EventManager.UIBus.AddListener<GameLeaved>(OnGameLeaved);
    }

    #endregion

    #region Bus callbacks

    private async void OnPlayerDead(PlayerDead pPlayerDead) 
    {
        await Task.Delay((int)(_playerSettings.TimeBeforeReciclePlayer*1000));
        _originPool.RecycleGameObject(gameObject);
    }

    private void OnGameLeaved(GameLeaved pGameLeaved) 
    {
        _originPool.RecycleGameObject(gameObject);
    }

    #endregion
}
