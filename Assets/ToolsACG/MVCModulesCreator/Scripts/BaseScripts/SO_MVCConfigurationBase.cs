using UnityEngine;

namespace ToolsACG.MVCModulesCreator.Bases
{
    public abstract class SO_MVCConfigurationBase : ScriptableObject
    {
        #region Values

        [Header("Aparition times")]

        [SerializeField] private float _delayBeforeEnter;
        public float DelayBeforeEnter => _delayBeforeEnter;

        [SerializeField] private float _delayBeforeExit;
        public float DelayBeforeExit => _delayBeforeExit;

        [SerializeField] private float _transitionDuration = 0.3f;
        public float TransitionDuration => _transitionDuration;

        
        [Header("View Initial Configuration")]

        [SerializeField] private float _viewInitialAlpha = 1;
        public float ViewInitialAlpha => _viewInitialAlpha;

        [SerializeField] private Vector3 _viewInitialScale = Vector3.one;
        public Vector3 ViewInitialScale => _viewInitialScale;

        [SerializeField] private bool _viewInitialState = true;
        public bool ViewInitialState => _viewInitialState;

        #endregion
    }
}
