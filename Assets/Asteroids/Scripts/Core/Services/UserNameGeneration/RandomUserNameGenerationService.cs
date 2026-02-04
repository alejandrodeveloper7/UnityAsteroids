using System;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using UnityEngine;

namespace Asteroids.Core.Services
{
    public class RandomUserNameGenerationService : InstancesServiceBase, IUserNameGenerationService
    {
        #region Fields

        [Header("Values")]
        private readonly string _posibleCharacters;
        private readonly string _baseName;
        private readonly int _charactersAmount;

        #endregion

        #region Constructors

        public RandomUserNameGenerationService(string posibleCharacters, string baseName, int charactersAmount)
        {
            _posibleCharacters = posibleCharacters;
            _baseName = baseName;
            _charactersAmount = charactersAmount;

            Initialize();
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method called in the constructor to initialize the Service
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: clean here all the elements that need be clean when the Service is destroyed
        }

        #endregion

        #region public Methods

        public string GetUsername()
        {
            string username;

            try
            {
                username = Environment.UserName;

                if (string.IsNullOrWhiteSpace(username))
                    throw new Exception();
            }
            catch
            {
                username = GenerateRandomName();
            }

            return username;
        }

        #endregion

        #region Private methods

        private string GenerateRandomName()
        {
            string name = _baseName;

            for (int i = 0; i < _charactersAmount; i++)
            {
                int index = UnityEngine.Random.Range(0, _posibleCharacters.Length);
                name += _posibleCharacters[index];
            }
            return name;
        }

        #endregion
    }

}