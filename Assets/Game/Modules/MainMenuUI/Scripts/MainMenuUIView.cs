using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.MainMenuUI
{
    public class MainMenuUIView : ModuleView
    {
        #region Fields     

        [Header("MVC References")]
        private MainMenuUIController _controller;
        private MainMenuUIModel _model;
        private SO_MainMenuUIConfiguration _configuration;

        [SerializeField] private GameObject _generalContainer;
        [Space]
        [SerializeField] private TMP_InputField _userNameInputField;
        public string UserName { get { return _userNameInputField.text; } }
        [Space]
        [SerializeField] private Button _playButton;

        #endregion

        #region Monobehaviour        

        protected override void Awake()
        {
            base.Awake();

            GetReferences();
            RegisterListeners();
        }

        private void OnDestroy()
        {
            UnRegisterListeners();
        }

        #endregion

        #region Initialization

        protected override void GetReferences()
        {
            _controller = GetComponent<MainMenuUIController>();
            _model = _controller.Model;
            _configuration = _controller.ModuleConfigurationData;
        }

        protected override void RegisterListeners()
        {
            _userNameInputField.onValueChanged.AddListener(OnUserNameInputValueChanged);

            _model.OnUserNameUpdated += OnUserNameUpdated;
        }

        protected override void UnRegisterListeners()
        {
            _userNameInputField.onValueChanged.RemoveListener(OnUserNameInputValueChanged);

            _model.OnUserNameUpdated -= OnUserNameUpdated;
        }

        #endregion

        #region Model Callbacks

        private void OnUserNameUpdated(string pUserName)
        {
            SetUserNameValue(pUserName);
        }

        #endregion

        #region View Methods   

        private void SetUserNameValue(string pValue)
        {
            _userNameInputField.text = pValue;
        }

        private void OnUserNameInputValueChanged(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
                _playButton.interactable = false;
            else
                _playButton.interactable = true;
        }

        #endregion

        #region Public Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public async void EnterTransition(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(0);
            TurnGeneralContainer(true);
            DoFadeTransition(1, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            pOnComplete?.Invoke();
        }

        public async void ExitTransion(float pDelay, float pTransitionDuration, Action pOnComplete = null)
        {
            await Task.Delay((int)(pDelay * 1000));
            SetViewAlpha(1);
            DoFadeTransition(0, pTransitionDuration);
            await Task.Delay((int)(pTransitionDuration * 1000));
            TurnGeneralContainer(false);
            pOnComplete?.Invoke();
        }

        #endregion
    }
}