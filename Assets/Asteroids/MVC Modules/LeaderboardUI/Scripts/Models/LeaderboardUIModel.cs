using ACG.Tools.Runtime.MVCModulesCreator.Bases;
using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using Asteroids.Core.Services;
using Asteroids.MVC.LeaderboardUI.ScriptableObjects;
using System;
using UnityEngine;
using Zenject;

namespace Asteroids.MVC.LeaderboardUI.Models
{
    public class LeaderboardUIModel : MVCModelBase
    {
        #region Fields and Properties

        [Header("references")]
        private readonly IContainerRuntimeDataService _containerRuntimeDataService;

        [Header("Data")]
        private readonly SO_LeaderboardUIConfiguration _configuration;
        [Space]
        private Leaderboard _leaderboardData;

        public string Username { get; private set; }

        #endregion

        #region Events

        public event Action<Leaderboard> LeaderboardDataUpdated;

        #endregion

        #region Constructors

        [Inject]
        public LeaderboardUIModel(SO_LeaderboardUIConfiguration configuration ,IContainerRuntimeDataService containerRuntimeDataService)
        {
            _configuration = configuration;
            _containerRuntimeDataService = containerRuntimeDataService;
            
            UpdateUsername();
        }

        #endregion

        #region Username management

        public void UpdateUsername()
        {
            Username = _containerRuntimeDataService.Data.UserName;
        }

        #endregion

        #region Leaderboard Data Management

        public void SetLeaderboardData(Leaderboard data)
        {
            _leaderboardData = data;
            LeaderboardDataUpdated?.Invoke(_leaderboardData);
        }

        #endregion

    }
}