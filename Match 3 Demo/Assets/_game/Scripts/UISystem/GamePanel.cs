using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _game.Scripts
{
    public class GamePanel : IPanel
    {
        [Title("Match Result")]
        [SerializeField] private RectTransform m_scorePanel;
        [SerializeField] private Button m_menuButton;
        [SerializeField] private TextMeshProUGUI m_resultText;
        
        private Tween _moveTween;
        private RectTransform _scorePanelTransform;
        
        [Inject] private PanelController _panelController;
        [Inject] private GameManager _gameManager;
        protected override void OnAwake()
        {
            m_menuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnEnable()
        {
            EventManager.TimerStart += HideScorePanel;
            EventManager.MatchEnd += ShowScorePanel;
        }

        private void OnDisable()
        {
            EventManager.TimerStart += HideScorePanel;
            EventManager.MatchEnd -= ShowScorePanel;
        }

        private void ShowScorePanel()
        {
            m_resultText.SetText(_gameManager.IsWin ? "YOU WIN" : "YOU LOSE");
            _moveTween = m_scorePanel.DOAnchorPos(new Vector2(0,0), 0.5f);
        }
        private void HideScorePanel()
        { 
            m_scorePanel.anchoredPosition = new Vector2(1500,0);
        }
        
        private void OnMenuButtonClicked()
        {
            _moveTween?.Kill();
            _moveTween = m_scorePanel.DOAnchorPos(new Vector2(1500,0), 0.5f);
            _moveTween.OnComplete(async () =>
            {
                _panelController.Show(PanelType.Menu);
                await _gameManager.QuitMatch();
            });
        }
    }
}
