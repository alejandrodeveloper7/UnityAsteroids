using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes
{
    public abstract class ModuleController : MonoBehaviour
    {
        #region Fields

        protected Dictionary<string, Delegate> Actions = new Dictionary<string, Delegate>();

        #endregion

        #region Protected Methods

        protected virtual void Awake()
        {
        }
        protected abstract void RegisterActions();
        protected abstract void UnRegisterActions();
        protected abstract void GetReferences();
        protected abstract void Initialize();
        protected abstract void CreateModel();

        #endregion

        #region UI callbacks

        public virtual void OnClick(Button pButton)
        {
            if (Actions.TryGetValue(pButton.name, out var action) && action is Action<Button> typedAction)
                typedAction.Invoke(pButton);
        }

        public virtual void OnValueChanged(Toggle pToggle)
        {
            if (Actions.TryGetValue(pToggle.name, out var action) && action is Action<Toggle> typedAction)
                typedAction.Invoke(pToggle);
        }

        public virtual void OnValueChanged(Slider pSlider)
        {
            if (Actions.TryGetValue(pSlider.name, out var action) && action is Action<Slider> typedAction)
                typedAction.Invoke(pSlider);
        }

        public virtual void OnValueChanged(TMP_InputField pInputField)
        {
            if (Actions.TryGetValue(pInputField.name, out var action) && action is Action<TMP_InputField> typedAction)
                typedAction.Invoke(pInputField);
        }

        public virtual void OnValueChanged(TMP_Dropdown pDropDown)
        {
            if (Actions.TryGetValue(pDropDown.name, out var action) && action is Action<TMP_Dropdown> typedAction)
                typedAction.Invoke(pDropDown);
        }

        public virtual void OnValueChanged(Scrollbar pScrollbar)
        {
            if (Actions.TryGetValue(pScrollbar.name, out var action) && action is Action<Scrollbar> typedAction)
                typedAction.Invoke(pScrollbar);
        }

        public virtual void OnValueChanged(ScrollRect pScrollRect)
        {
            if (Actions.TryGetValue(pScrollRect.name, out var action) && action is Action<ScrollRect> typedAction)
                typedAction.Invoke(pScrollRect);
        }
        #endregion
    }
}