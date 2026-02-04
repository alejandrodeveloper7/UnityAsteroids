using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ACG.Scripts.UIUtilitys
{
    public class UILayoutUpdater : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [SerializeField] private HorizontalOrVerticalLayoutGroup[] _layoutGroups;
        [SerializeField] private ContentSizeFitter[] _contentsSizeFitters;

        #endregion

        #region Functionality

        public async void UpdateLayout()
        {
            UpdateContentSizeFitters();
            await Task.Yield();
            UpdateLayoutGroups();
        }

        private void UpdateContentSizeFitters()
        {
            if (_contentsSizeFitters != null)
                foreach (ContentSizeFitter contentSizeFitter in _contentsSizeFitters)
                    if (contentSizeFitter != null)
                        LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.GetComponent<RectTransform>());

        }

        private void UpdateLayoutGroups()
        {
            if (_layoutGroups != null)
                foreach (HorizontalOrVerticalLayoutGroup layoutGroup in _layoutGroups)
                    if (layoutGroup != null)
                        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }

        #endregion
    }
}