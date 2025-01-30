using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes
{
    public abstract class ModuleController : MonoBehaviour
    {
        protected Dictionary<string, Action> m_actions = new Dictionary<string, Action>();

        protected virtual void Awake()
        {
            RegisterActions();
        }

        public virtual void OnClick(Button button)
        {
            if (m_actions.ContainsKey(button.name))
                m_actions[button.name].Invoke();
        }

        public virtual void OnValueChanged(Toggle toggle)
        {
            if (m_actions.ContainsKey(toggle.name))
                m_actions[toggle.name].Invoke();
        }

        public virtual void OnValueChanged(Slider slider)
        {
            if (m_actions.ContainsKey(slider.name))
                m_actions[slider.name].Invoke();
        }

        protected abstract void RegisterActions();
        protected abstract void SetData();

    }
}