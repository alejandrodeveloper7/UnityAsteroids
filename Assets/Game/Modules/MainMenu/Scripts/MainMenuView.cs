using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsACG.Scenes.MainMenu
{
    public interface IMainMenuView
    {
        void TurnGeneralContainer(bool pState);

        void SetUserNameValue(string pValue);
        string UserName { get; }
    }

    public class MainMenuView : ModuleView, IMainMenuView
    {
        #region Fields        

        [SerializeField] private GameObject _generalContainer;
        [Space]
        [SerializeField] private TMP_InputField _userNameInputField;
        public string UserName
        {
            get
            {
                return _userNameInputField.text;
            }
        }
        [Space]
        [SerializeField] private Button _playButton;

        #endregion

        #region Protected Methods     

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            _userNameInputField.onValueChanged.AddListener(OnUserNameInputValueChanged);
        }

        private void OnDisable()
        {
            _userNameInputField.onValueChanged.RemoveListener(OnUserNameInputValueChanged);
        }
        #endregion

        #region View Methods

        public void TurnGeneralContainer(bool pState)
        {
            _generalContainer.SetActive(pState);
        }

        public void SetUserNameValue(string pValue)
        {
            _userNameInputField.text = pValue;
        }

        #endregion

        #region Private Methods

        private void OnUserNameInputValueChanged(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
                _playButton.interactable = false;
            else
                _playButton.interactable = true;
        }

        #endregion
    }
}