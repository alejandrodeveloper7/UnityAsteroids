using ToolsACG.Utils.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISoundTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField]private SO_Sound clickSound;
    [SerializeField] private SO_Sound hoverSound;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            EventManager.GetUiBus().RaiseEvent(new GenerateUISound() { SoundsData = clickSound });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
            EventManager.GetUiBus().RaiseEvent(new GenerateUISound() { SoundsData = hoverSound });
    }
}