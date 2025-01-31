using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    [SerializeField] private Image _itemDisplay;

    private List<SO_BaseElement> _availableItems;
    private int _selectedIndex = 0;
    public int SelectedIndex 
    { 
        get {
            int validIndex = (_selectedIndex % _availableItems.Count + _availableItems.Count) % _availableItems.Count;
            return validIndex; 
        }
    }


    public void SetData(List<SO_BaseElement> pItems, int pInitialIndex = 0)
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
