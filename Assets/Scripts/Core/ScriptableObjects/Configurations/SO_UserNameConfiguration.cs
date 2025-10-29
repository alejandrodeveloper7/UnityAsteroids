using ToolsACG.SOCreator.Configurations;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "UserNameConfiguration", menuName = "ScriptableObjects/Configurations/UserName")]
    public class SO_UserNameConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("User name")]

        [SerializeField] private string _baseName = "User";
        public string BaseName => _baseName;

        [SerializeField] int _nameRandomCharactersAmount = 6;
        public int NameRandomCharactersAmount => _nameRandomCharactersAmount;

        [SerializeField] private string _namePosibleCharacters = "0123456789";
        public string NamePosibleCharacters => _namePosibleCharacters;

        #endregion
    }
}
