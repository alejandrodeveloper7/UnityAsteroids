using ToolsACG.Utils.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField]private SO_Sound[] clickSound;
    [SerializeField] private SO_Sound[] hoverSound;

    #region Interface Handlers

    public void OnPointerClick(PointerEventData pEventData)
    {
        if (pEventData.button is PointerEventData.InputButton.Left is false)
            return;

        if (clickSound != null)
            EventManager.GetGameplayBus().RaiseEvent(new Generate2DSound() { SoundsData = clickSound });
    }

    public void OnPointerEnter(PointerEventData pEventData)
    {
        if (hoverSound != null)
            EventManager.GetGameplayBus().RaiseEvent(new Generate2DSound() { SoundsData = hoverSound });
    }

    #endregion
}