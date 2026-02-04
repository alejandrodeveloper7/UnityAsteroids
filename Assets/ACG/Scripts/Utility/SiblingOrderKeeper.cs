using UnityEngine;

namespace ACG.Scripts.Utilitys
{
    public class SiblingOrderKeeper : MonoBehaviour
    {
        #region Enums

        public enum SiblingPosition
        {
            First,
            Last
        }

        #endregion

        #region Fields

        [Header("Configuration")]
        [SerializeField] private int _updateEveryNFrames = 2;
        [SerializeField] private SiblingPosition _targetPosition = SiblingPosition.Last;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            if (_updateEveryNFrames <= 0)
                _updateEveryNFrames = 1;

            if (transform.parent == null)
            {
                Debug.LogWarning($"{nameof(SiblingOrderKeeper)} in {gameObject.name} has no parent, component disabled.");
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            if (Time.frameCount % _updateEveryNFrames is not 0)
                return;

            switch (_targetPosition)
            {
                case SiblingPosition.First:
                    if (transform.GetSiblingIndex() is not 0)
                        transform.SetAsFirstSibling();
                    break;

                case SiblingPosition.Last:
                    if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
                        transform.SetAsLastSibling();
                    break;
            }
        }

        #endregion
    }
}