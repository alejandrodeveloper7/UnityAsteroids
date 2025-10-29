using System.Collections.Generic;
using ToolsACG.SOCreator.Data;
using UnityEngine;

namespace ToolsACG.SOCreator.Collections
{
    public class SO_CollectionBase<T> : ScriptableObject where T : SO_DataBase
    {
        #region Values

        [Header("References")]

        [SerializeField] private List<T> _elements;
        public List<T> Elements => _elements;

        #endregion

        #region Methods

        public T GetElementById(int id)
        {
            foreach (T item in _elements)
                if (item.Id == id)
                    return item;

            return null;
        }

        #endregion
    }
}