using System;
using System.Linq;
using _game.Scripts.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _game.Scripts.UISystem
{
    public class RegisterPanel : IPanel
    {
        [SerializeField] private ToggleGroup m_toggleGroup;
        [SerializeField] private TMP_InputField m_inputField;
        [SerializeField] private Button m_registerButton;
        
        private Toggle CurrentSelection => m_toggleGroup.ActiveToggles().FirstOrDefault();

        [Inject] private IStorage _storage;
        [Inject] private PanelController _panelController;
        protected override void OnAwake()
        {
            m_registerButton.onClick.AddListener(OnRegisterClick);
        }

        private void OnRegisterClick()
        {
            if (string.IsNullOrEmpty(m_inputField.text)) return;

            SavePlayerData();
            _panelController.Show(PanelType.Menu);
            
        }

        private void SavePlayerData()
        {
            _storage.Data.PlayerData.UserName = m_inputField.text;
            _storage.Data.PlayerData.Gender = (Gender)Enum.Parse(typeof(Gender), CurrentSelection.name);
            _storage.Data.PlayerData.IsRegistered = true;
        }
        
    }
}
