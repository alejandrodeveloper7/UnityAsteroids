using System.Threading.Tasks;
using UnityEngine;

namespace Asteroids.MVC.PlayerHealthBarUI.Views
{
    public interface IPlayerHealthBarUIView
    {
        void SetShieldSliderColors(Color recoveringColor, Color FullColor, Color ShineColor);

        void SetMaxPosibleHealthValue(int amount);
        void SetMaxPosibleShieldSliderValue(float value);

        void SetCurrentHealth(int amount);
        void SetShieldSliderValue(float value);

        Task PlayShieldLostSliderTransition(float value);
    }
}