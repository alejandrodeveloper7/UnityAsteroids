using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes
{
    public abstract class ModuleController : MonoBehaviour
    {
        protected Dictionary<string, Action> Actions = new Dictionary<string, Action>();
        protected ModuleView View;

        protected virtual void Awake()
        {
            View = GetComponent<ModuleView>();
            Initialize();
            RegisterActions();
        }

        public virtual void OnClick(Button pButton)
        {
            if (Actions.ContainsKey(pButton.name))
                Actions[pButton.name].Invoke();
        }

        public virtual void OnValueChanged(Toggle pToggle)
        {
            if (Actions.ContainsKey(pToggle.name))
                Actions[pToggle.name].Invoke();
        }

        public virtual void OnValueChanged(Slider pSlider)
        {
            if (Actions.ContainsKey(pSlider.name))
                Actions[pSlider.name].Invoke();
        }

        public virtual void OnValueChanged(TMP_InputField pInputField)
        {
            if (Actions.ContainsKey(pInputField.name))
                Actions[pInputField.name].Invoke();
        }

        public virtual void OnValueChanged(TMP_Dropdown pDropDown)
        {
            if (Actions.ContainsKey(pDropDown.name))
                Actions[pDropDown.name].Invoke();
        }

        protected abstract void RegisterActions();
        protected abstract void Initialize();
    }
}