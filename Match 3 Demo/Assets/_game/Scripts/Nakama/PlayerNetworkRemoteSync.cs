using System;
using System.Collections.Generic;
using System.Text;
using _game.Scripts;
using _game.Scripts.Storage;
using Nakama;
using Nakama.TinyJson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerNetworkRemoteSync : MonoBehaviour
{
    public RemotePlayerNetworkData NetworkData;
    public int Score => _score;

    [SerializeField] private TextMeshProUGUI m_nameText;
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private Image m_avatar;
   
    private int _score;
    private string _userName;
    
    [Inject] private GameManager _gameManager;
    [Inject] private ProjectSettings _projectSettings;

    private void OnEnable()
    {
        _gameManager.NakamaConnection.Socket.ReceivedMatchState += EnqueueOnReceivedMatchState;
    }

    private void OnDisable()
    {
        _gameManager.NakamaConnection.Socket.ReceivedMatchState -= EnqueueOnReceivedMatchState;
    }
    
    private void EnqueueOnReceivedMatchState(IMatchState matchState)
    {
        var mainThread = UnityMainThreadDispatcher.Instance();
        mainThread.Enqueue(() => OnReceivedMatchState(matchState));
    }
    
    private void OnReceivedMatchState(IMatchState matchState)
    {
        if (matchState.UserPresence.SessionId != NetworkData.User.SessionId)
        {
            return;
        }
        
        switch (matchState.OpCode)
        {
            case OpCodes.Score:
                UpdateScore(matchState.State);
                break;
            case OpCodes.Gender:
                SetAvatar(matchState.State);
                break;
            case OpCodes.Name:
                SetName(matchState.State);
                break;
        }
    }

    private void SetName(byte[] state)
    {
        var stateDictionary = GetStateAsDictionary(state);
        _userName = stateDictionary["name"];
        m_nameText.SetText(_userName);
    }
    private void SetAvatar(byte[] state)
    {
        var stateDictionary = GetStateAsDictionary(state);
        var gender = stateDictionary["gender"];
        var avatar = (Gender)Enum.Parse(typeof(Gender), gender);

        m_avatar.sprite = avatar == Gender.Boy ? _projectSettings.BoyAvatar : _projectSettings.GirlAvatar;
    }
    private void UpdateScore(byte[] state)
    {
        var stateDictionary = GetStateAsDictionary(state);
        _score = Int32.Parse((stateDictionary["score"]));
        m_scoreText.SetText(_score.ToString());
    }
    private IDictionary<string, string> GetStateAsDictionary(byte[] state)
    {
        return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
    }
}
