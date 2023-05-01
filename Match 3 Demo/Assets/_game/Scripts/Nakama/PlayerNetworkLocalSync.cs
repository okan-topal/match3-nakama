using System;
using _game.Scripts;
using _game.Scripts.Storage;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerNetworkLocalSync : MonoBehaviour
{
    [ShowInInspector, ReadOnly] private int _currentScore;
    
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_nameText;
    [SerializeField] private Image m_avatar;
    
    private string _userName;
    private Gender _gender;
    
    [Inject] private GameManager _gameManager;
    [Inject] private IStorage _storage;
    [Inject] private ProjectSettings _projectSettings;
    private void OnEnable()
    {
        EventManager.ScoreChange += SendMatchScore;
    }
    
    private void OnDisable()
    {
        EventManager.ScoreChange -= SendMatchScore;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _userName = _storage.Data.PlayerData.UserName;
        _gender = _storage.Data.PlayerData.Gender;
        
        m_nameText.SetText(_userName);
        m_avatar.sprite = _gender == Gender.Boy ? _projectSettings.BoyAvatar : _projectSettings.GirlAvatar;
        
        SendPlayerData();
    }

    private void SendPlayerData()
    {
        _gameManager.SendMatchInfo(OpCodes.Name,MatchDataJson.Name(_userName));
        _gameManager.SendMatchInfo(OpCodes.Gender,MatchDataJson.Gender(_gender.ToString()));
    }
    
    private void SendMatchScore(int score)
    {
        m_scoreText.SetText(score.ToString());
        
        _gameManager.SendMatchInfo(OpCodes.Score,MatchDataJson.Score(score));
    }
}
