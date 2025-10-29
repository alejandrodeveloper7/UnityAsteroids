using ToolsACG.SOCreator.Interfaces;
using UnityEngine;

namespace ToolsACG.SOCreator.Data
{
    public abstract class SO_DataBase : ScriptableObject , IData
    {
        #region Values

        [Header("Base Configuration")]

        [SerializeField] private int _id;
        public int Id => _id;

        #endregion
    }
}