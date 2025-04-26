using DG.Tweening;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    #region Fields

    private BackgroundConfiguration _backgroundConfiguration;
    [Space]
    [SerializeField] private SpriteRenderer _backgroundSprite;
    private Transform _backgroundTransform;

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _backgroundConfiguration = ResourcesManager.Instance.GetScriptableObject<BackgroundConfiguration>(ScriptableObjectKeys.BACKGROUND_CONFIGURATION_KEY);
        _backgroundTransform = _backgroundSprite.transform;
    }

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
        SetAlphaValue(_backgroundConfiguration.InitialAlphaValue);
    }

    private void OnEnable()
    {
        EventManager.UIBus.AddListener<StartGame>(OnStartGame);
    }

    private void OnDisable()
    {
        EventManager.UIBus.RemoveListener<StartGame>(OnStartGame);
    }

    #endregion

    #region Bus Callbacks

    private void OnStartGame(StartGame pStartGame)
    {
        DoFadeTransition(_backgroundConfiguration.FinalAlphaValue, _backgroundConfiguration.AlphaTransitionDuration);
        StartMovement();
    }

    #endregion
     
    #region Alpha Management

    private void SetAlphaValue(float pValue)
    {
        _backgroundSprite.color = new Color(_backgroundSprite.color.r, _backgroundSprite.color.g, _backgroundSprite.color.b, pValue);
    }

    private void DoFadeTransition(float pValue, float pDuration)
    {
        _backgroundSprite.DOKill();
        _backgroundSprite.DOFade(pValue, pDuration);
    }

    #endregion

    #region Movement Management

    private void StartMovement()
    {
        DOTween.Sequence()
     .Append(_backgroundTransform.DOLocalMove(_backgroundConfiguration.FinalPosition, _backgroundConfiguration.MovementDuration).SetEase(_backgroundConfiguration.MovementEase))
     .Append(_backgroundTransform.DOLocalMove(_backgroundConfiguration.InitialPosition, _backgroundConfiguration.MovementDuration).SetEase(_backgroundConfiguration.MovementEase))
     .SetLoops(-1, LoopType.Yoyo);
    }

    #endregion

}
