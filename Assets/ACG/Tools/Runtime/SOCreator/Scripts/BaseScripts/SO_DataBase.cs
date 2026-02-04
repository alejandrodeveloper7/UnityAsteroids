using ACG.Tools.Runtime.SOCreator.Interfaces;
using UnityEngine;

namespace ACG.Tools.Runtime.SOCreator.Data
{
    public abstract class SO_DataBase : ScriptableObject, IData
    {
        #region Values

        [Header("General")]

        [SerializeField] private int _id;
        public int Id => _id;

        #endregion
    }
}