using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ACG.Tools.Runtime.MVCModulesCreator.Bases
{
    public abstract class MVCControllerBase : MonoBehaviour
    {
        #region Fields

        protected Dictionary<string, Delegate> Actions = new();
        protected MVCViewBase ViewBase;

        #endregion

        #region Initialization

        protected virtual void GetReferences()
        {
            ViewBase = GetComponent<MVCViewBase>();
        }
        protected abstract void Initialize();
        protected abstract void RegisterActions();
        protected abstract void UnRegisterActions();

        #endregion

        #region Monobehaviour

        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void OnDestroy() { }

        #endregion

        #region UI callbacks

        public virtual void OnClick(Button button)
        {
            if (Actions.TryGetValue(button.name, out var action) && action is Action<Button> typedAction)
                typedAction.Invoke(button);
        }

        public virtual void OnValueChanged(Toggle toggle)
        {
            if (Actions.TryGetValue(toggle.name, out var action) && action is Action<Toggle> typedAction)
                typedAction.Invoke(toggle);
        }

        public virtual void OnValueChanged(Slider slider)
        {
            if (Actions.TryGetValue(slider.name, out var action) && action is Action<Slider> typedAction)
                typedAction.Invoke(slider);
        }

        public virtual void OnValueChanged(TMP_InputField inputField)
        {
            if (Actions.TryGetValue(inputField.name, out var action) && action is Action<TMP_InputField> typedAction)
                typedAction.Invoke(inputField);
        }

        public virtual void OnValueChanged(TMP_Dropdown dropDown)
        {
            if (Actions.TryGetValue(dropDown.name, out var action) && action is Action<TMP_Dropdown> typedAction)
                typedAction.Invoke(dropDown);
        }

        public virtual void OnValueChanged(Scrollbar scrollbar)
        {
            if (Actions.TryGetValue(scrollbar.name, out var action) && action is Action<Scrollbar> typedAction)
                typedAction.Invoke(scrollbar);
        }

        public virtual void OnValueChanged(ScrollRect scrollRect)
        {
            if (Actions.TryGetValue(scrollRect.name, out var action) && action is Action<ScrollRect> typedAction)
                typedAction.Invoke(scrollRect);
        }

        #endregion
    }
}