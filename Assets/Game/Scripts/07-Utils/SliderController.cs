using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Slider _slider;
    
    //[Header("Actions")]
    private Action OnSliderFilled { get; set; }

    //[Header("Properties")]
    public float Value { get { return _slider.value; } }
    public float MaxValue { get { return _slider.maxValue; } }

    [Header("Cache")]
    private Tween _currentTween;

    #endregion

    #region General Management

    public void SetValues(float pMaxValue, float pCurrentValue)
    {
        _slider.maxValue = pMaxValue;
        _slider.value = pCurrentValue;
    }

    public void Restart()
    {
        StopMovement();
        _slider.value = 0;
    }

    #endregion

    #region Movement Management

    public void SetValueInstantly(float pValue)
    {
        StopMovement();

        _slider.value = pValue;
        CheckSliderFilled();
    }
    
    public void SetValueProgresively(float pValue, float pTime)
    {
        StopMovement();

        _currentTween = _slider.DOValue(pValue, pTime)
            .OnComplete(CheckSliderFilled);
    }

    private void StopMovement()
    {
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
            _currentTween = null;
        }
    }

    #endregion

    #region Slider Filled Callback

    public void SetSliderFilledCallback(Action pAction)
    {
        OnSliderFilled += pAction;
    }
    public void RemoveSliderFilledCallback(Action pAction)
    {
        OnSliderFilled -=pAction;
    }
    public void RemoveAllSliderFilledCallbacks()
    {
        OnSliderFilled = null;
    }
    private void CheckSliderFilled()
    {
        if (Value < MaxValue)
            return;

        OnSliderFilled?.Invoke();
    }

    #endregion
}
