using ACG.Tools.Runtime.SOCreator.Configurations;
using UnityEngine;

namespace ACG.Scripts.ScriptableObjects.Configurations
{
   [CreateAssetMenu(fileName = "RandomUserNameGenerationConfiguration", menuName = "ScriptableObjects/Configurations/RandomUserNameGeneration")]
   public class SO_RandomUserNameGenerationConfiguration : SO_ConfigurationBase
   {
        #region Values

        [Header("Configuration")]

        [SerializeField] private string _baseName = "User";
        public string BaseName => _baseName;

        [Space]

        [SerializeField] private string _randomCharacters = "0123456789";
        public string RandomCharacters => _randomCharacters;

        [SerializeField] int _randomCharactersAmount = 6;
        public int RandomCharactersAmount => _randomCharactersAmount;

        #endregion
    }
}
