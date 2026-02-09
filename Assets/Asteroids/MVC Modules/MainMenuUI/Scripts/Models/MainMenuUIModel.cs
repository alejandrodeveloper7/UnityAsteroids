using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.Core.Interfaces;
using Asteroids.Core.ScriptableObjects.Collections;
using Asteroids.Core.ScriptableObjects.Data;
using Asteroids.Core.Services;
using Asteroids.MVC.MainMenuUI.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.MainMenuUI.Models
{
    public class MainMenuUIModel : MVCModelBase
    {
        #region Fields and Properties

        public string UserName { get; private set; }
        public SO_ShipData SelectedShip { get; private set; }
        public SO_BulletData SelectedBullet { get; private set; }

        [Header("References")]
        private readonly IUserNameGenerationService _userNameGenerationService;

        [Header("Data")]
        private readonly SO_MainMenuUIConfiguration _configuration;
        [Space]
        private readonly SO_BulletsCollection _bulletsCollection;
        private readonly SO_ShipsCollection _shipsCollection;

        #endregion

        #region Events

        public event Action<string> UserNameUpdated;

        public event Action<List<ISelectorElement>> ShipsInitialized;
        public event Action<SO_ShipData> ShipSelected;

        public event Action<List<ISelectorElement>> BulletsInitialized;
        public event Action<SO_BulletData> BulletSelected;

        #endregion

        #region Constructors

        [Inject]
        public MainMenuUIModel(SO_MainMenuUIConfiguration configuration, IUserNameGenerationService usernameGenerationService, SO_BulletsCollection bulletsCollection, SO_ShipsCollection shipsCollection)
        {
            _configuration = configuration;
            _userNameGenerationService = usernameGenerationService;
            _bulletsCollection = bulletsCollection;
            _shipsCollection = shipsCollection;
        }

        #endregion

        #region Initialization

        public void Initialize()
        {
            InitializeUserName();
            InitializeBullets();
            InitializeShips();
        }

        #endregion

        #region Name Management

        private void InitializeUserName()
        {
            UserName = _userNameGenerationService.GetUsername();
            UserNameUpdated?.Invoke(UserName);
        }

        public void SetUserName(string newName)
        {
            UserName = newName;
            UserNameUpdated?.Invoke(UserName);
        }

        #endregion

        #region Ships Management

        private void InitializeShips()
        {
            List<ISelectorElement> CastedShipsCollection = _shipsCollection.Elements.Cast<ISelectorElement>().ToList();
            ShipsInitialized?.Invoke(CastedShipsCollection);
        }

        public void SetShipSelectedId(int id)
        {
            SelectedShip = _shipsCollection.GetElementById(id);
            ShipSelected?.Invoke(SelectedShip);
        }

        #endregion

        #region Bullets Management

        private void InitializeBullets()
        {
            List<ISelectorElement> CastedBulletsCollection = _bulletsCollection.Elements.Cast<ISelectorElement>().ToList();
            BulletsInitialized?.Invoke(CastedBulletsCollection);
        }

        public void SetBulletSelectedId(int id)
        {
            SelectedBullet = _bulletsCollection.GetElementById(id);
            BulletSelected?.Invoke(SelectedBullet);
        }

        #endregion
    }
}