using ACG.Scripts.ScriptableObjects.Configurations;
using ACG.Tools.Runtime.ServicesCreator.Bases;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.Core.Services
{
    public class RandomUserNameGenerationService : InstancesServiceBase, IUserNameGenerationService
    {
        #region Fields

        [Header("References")]
        private readonly SO_RandomUserNameGenerationConfiguration _configuration;

        #endregion

        #region Constructors

        [Inject]
        public RandomUserNameGenerationService(SO_RandomUserNameGenerationConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Initialization     

        public override void Initialize()
        {
            base.Initialize();
            // TODO: Method for initial logic and event subscriptions (called by Zenject)
        }

        public override void Dispose()
        {
            base.Dispose();
            // TODO: Clean here all the listeners or elements that need be clean when the script is destroyed (called by Zenject)
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
            string name = _configuration.BaseName;

            for (int i = 0; i < _configuration.RandomCharactersAmount; i++)
            {
                int index = UnityEngine.Random.Range(0, _configuration.RandomCharacters.Length);
                name += _configuration.RandomCharacters[index];
            }
            return name;
        }

        #endregion
    }

}