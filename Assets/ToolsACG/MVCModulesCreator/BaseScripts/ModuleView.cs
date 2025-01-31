using UnityEngine;

namespace ToolsACG.Scenes
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ModuleView : MonoBehaviour
    {
        internal CanvasGroup CanvasGroup;

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }
    }
}