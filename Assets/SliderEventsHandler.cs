using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEventsHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Slider _slider;
    private bool _isInteracting;
    [Space]
    public Action OnEndDrag;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }


    public void OnPointerDown(PointerEventData pEventData)
    {
        _isInteracting = true;
    }

    public void OnPointerUp(PointerEventData pEventData)
    {
        if (_isInteracting is false)
            return;

        _isInteracting = false;
        OnEndDrag?.Invoke();
    }
}