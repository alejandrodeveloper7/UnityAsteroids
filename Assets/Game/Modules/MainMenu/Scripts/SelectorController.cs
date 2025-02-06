using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    [SerializeField] private Image _itemDisplay;

    private List<SO_Selectable> _availableItems;
    private int _selectedIndex = 0;
    public int SelectedId 
    { 
        get 
        {
            int validIndex = (_selectedIndex % _availableItems.Count + _availableItems.Count) % _availableItems.Count;
            return _availableItems[validIndex].Id; 
        }
    }


    public void SetData(List<SO_Selectable> pItems, int pInitialIndex = 0)
    {
        _availableItems = pItems;
        _selectedIndex = pInitialIndex;
        DisplaySelectedItem(RightArrowButtonClick);
    }

    internal void DisplaySelectedItem(Action pOnNotActive)
    {
        int validIndex = (_selectedIndex % _availableItems.Count + _availableItems.Count) % _availableItems.Count;

        if (_availableItems[validIndex].IsActive)
            _itemDisplay.sprite = _availableItems[validIndex].Sprite;
        else
            pOnNotActive?.Invoke();
    }

    public void LeftArrowButtonClick()
    {
        _selectedIndex--;
        DisplaySelectedItem(LeftArrowButtonClick);
    }

    public void RightArrowButtonClick()
    {
        _selectedIndex++;
        DisplaySelectedItem(RightArrowButtonClick);
    }
}
