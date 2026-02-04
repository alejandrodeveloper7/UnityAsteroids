using UnityEngine;
using UnityEngine.UI;

namespace ACG.Scripts.UIControllers
{
    public class UIGridLayoutController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        private GridLayoutGroup _gridLayout;
        private RectTransform _gridLayoutRectTransform;

        [Header("States")]
        private bool _isActive;

        [Header("Data")]
        private int _columns;
        private int _rows;
        [Space]
        private int _totalCards;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            _gridLayout = GetComponent<GridLayoutGroup>();
            _gridLayoutRectTransform = _gridLayout.GetComponent<RectTransform>();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (_isActive)
                RecalculateCellsSize();
        }

        #endregion

        #region Management

        public void SetData(float rows, float columns)
        {
            _columns = Mathf.RoundToInt(columns);
            _rows = Mathf.RoundToInt(rows);
            _totalCards = _columns * _rows;

            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            _gridLayout.constraintCount = _rows;
        }

        public void SetActiveState(bool state)
        {
            _isActive = state;

            if (_isActive)
                RecalculateCellsSize();
        }

        #endregion

        #region Functionality

        public void RecalculateCellsSize()
        {
            if (_totalCards == 0)
                return;

            float unUsableHeight = (_gridLayout.spacing.y * (_rows - 1)) + _gridLayout.padding.top + _gridLayout.padding.bottom;
            float usableHeight = _gridLayoutRectTransform.rect.height - unUsableHeight;
            float cellHeight = usableHeight / _rows;

            float unUsableWidth = (_gridLayout.spacing.x * (_columns - 1)) + _gridLayout.padding.left + _gridLayout.padding.right;
            float usableWidth = _gridLayoutRectTransform.rect.width - unUsableWidth;
            float cellWidth = usableWidth / _columns;

            if (cellWidth < cellHeight)
                cellHeight = cellWidth;
            else
                cellWidth = cellHeight;

            _gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        }

        #endregion
    }
}