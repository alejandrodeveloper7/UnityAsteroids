using Asteroids.ApiCallers.DreamloLeaderboardApiCaller;
using System.Collections.Generic;

namespace Asteroids.MVC.LeaderboardUI.Views
{
    public interface ILeaderboardUIView
    {
        void RestartView();
   
        void DisplayLeaderboardError();
        void UpdateLeaderboardRowsData(List<LeaderboardEntry> data, string playerUserName);
    }
}