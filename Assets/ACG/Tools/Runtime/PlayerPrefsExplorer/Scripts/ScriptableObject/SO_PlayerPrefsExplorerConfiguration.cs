using UnityEngine;

namespace ACG.Tools.Runtime.PlayerPrefsExplorer.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerPrefsExplorerConfiguration", menuName = "ToolsACG/PlayerPrefsExplorer/PlayerPrefsExplorerConfiguration")]
    public class SO_PlayerPrefsExplorerConfiguration : ScriptableObject
    {
        #region Values

        [Header("Configuration")]

        public string WindowTitle = "PlayerPrefs Explorer";
        public Vector2 WindowMinSize = new(550, 400);

        #endregion
    }
}