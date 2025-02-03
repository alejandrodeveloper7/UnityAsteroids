using DG.Tweening;
using ToolsACG.Utils.Events;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    #region Fields

    [SerializeField] private BackgroundConfiguration _configuration;
    [Space]
    [SerializeField] private SpriteRenderer _backgroundSprite;
    private Transform _backgroundTransform;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();
        SetAlphaValue(_configuration.InitialAlphaValue);
        StartMovement();
    }

    private void OnEnable()
    {
        EventManager.GetUiBus().AddListener<StartGame>(OnStartGame);
    }

    private void OnDisable()
    {
        EventManager.GetUiBus().RemoveListener<StartGame>(OnStartGame);
    }

    #endregion

    #region Bus Callbacks

    private void OnStartGame(StartGame pStartGame)
    {
        DoFadeTransition(_configuration.AlphaValue, _configuration.AlphaTransitionDuration);
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _backgroundTransform = _backgroundSprite.transform;
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
     .Append(_backgroundTransform.DOLocalMove(_configuration.FinalPosition, _configuration.MovementDuration).SetEase(_configuration.MovementEase))
     .Append(_backgroundTransform.DOLocalMove(_configuration.InitialPosition, _configuration.MovementDuration).SetEase(_configuration.MovementEase))
     .SetLoops(-1, LoopType.Yoyo);
    }

    #endregion

}
