using ACG.Tools.Runtime.SOCreator.Configurations;
using DG.Tweening;
using UnityEngine;

namespace Asteroids.Core.ScriptableObjects.Configurations
{
    [CreateAssetMenu(fileName = "BackgroundConfiguration", menuName = "ScriptableObjects/Configurations/Background")]
    public class SO_BackgroundConfiguration : SO_ConfigurationBase
    {
        #region Values

        [Header("Positions")]

        [SerializeField]private Vector3 _initialPosition;
        public Vector3 InitialPosition=> _initialPosition;
     
        [SerializeField] private Vector3 _finalPosition;
        public Vector3 FinalPosition=> _finalPosition;


        [Header("Movement")]
       
        [SerializeField] private float _movementDuration;
        public float MovementDuration=> _movementDuration;
    
        [SerializeField] private Ease _movementEase;
        public Ease MovementEase=> _movementEase;


        [Header("Aparition")]
    
        [SerializeField] private float _initialAlphaValue;
        public float InitialAlphaValue=> _initialAlphaValue;
    
        [SerializeField] private float _finalAlphaValue;
        public float FinalAlphaValue=> _finalAlphaValue;

        [Space]

        [SerializeField] private float _alphaTransitionDuration;
        public float AlphaTransitionDuration=> _alphaTransitionDuration;
        
        #endregion
    }
}