using UnityEngine;

namespace ToolsACG.PlayerPrefsExplorer.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerPrefsExplorerConfiguration", menuName = "ToolsACG/PlayerPrefsExplorer/PlayerPrefsExplorerConfiguration")]
    public class SO_PlayerPrefsExplorerConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]
        public Vector2 WindowMinSize = new(400,400);
        public string keyDisplayFormat = "{0}  |  {1}";

        #endregion
    }
}