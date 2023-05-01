using System;
using System.Collections;
using System.Collections.Generic;
using _game.Scripts;
using _game.Scripts.Storage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuPanel : IPanel
{
  [SerializeField] private Button m_findMatchButton;
  [SerializeField] private TextMeshProUGUI m_statusText;
  [SerializeField] private Image m_avatarImage;
  [SerializeField] private TextMeshProUGUI m_userName;
  [SerializeField] private TextMeshProUGUI m_highScore;
  [SerializeField] private TextMeshProUGUI m_winCount;
  [SerializeField] private TextMeshProUGUI m_loseCount;

  private ScoreData _scoreData;
  private PlayerData _playerData;
  [Inject] private GameManager _gameManager;
  [Inject] private ProjectSettings _projectSettings;
  [Inject] private IStorage _storage;
  protected override void OnAwake()
  {
    m_findMatchButton.onClick.AddListener(OnFindMatchClick);
    _scoreData = _storage.Data.ScoreData;
    _playerData = _storage.Data.PlayerData;
  }

  protected override void OnShowed()
  {
    var gender = _playerData.Gender;
    var userName = _playerData.UserName;
    var highScore = _scoreData.HighScore;
    
    var winCount =_scoreData.WinCount;
    var loseCount = _scoreData.LoseCount;
    
    m_statusText.enabled = false;
    m_findMatchButton.gameObject.SetActive(true);
    
    m_avatarImage.sprite = gender switch
    {
      Gender.Boy => _projectSettings.BoyAvatar,
      Gender.Girl => _projectSettings.GirlAvatar,
      _ => m_avatarImage.sprite
    };
    
    m_userName.SetText(userName);
    m_highScore.SetText(highScore.ToString());
    
    m_winCount.SetText(winCount.ToString());
    m_loseCount.SetText(loseCount.ToString());
  }

  private void OnFindMatchClick()
  {
      m_findMatchButton.gameObject.SetActive(false);
      m_statusText.enabled = true;
     _gameManager.NakamaConnection.FindMatch();
  }
}
