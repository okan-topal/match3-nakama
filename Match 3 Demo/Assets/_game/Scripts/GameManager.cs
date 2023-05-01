using System.Collections.Generic;

using _game.Scripts.Storage;
using Nakama;
using UnityEngine;
using Zenject;

namespace _game.Scripts
{
    public partial class GameManager : MonoBehaviour
    {
        public bool IsWin => _isWin;


        private bool _isWin;
        [Inject] private PanelController _panelController;
        [Inject] private IStorage _storage;
        [Inject] private PlayerPanelFactory _playerPanelFactory;
        [Inject] private ScoreManager _scoreManager;
        [Inject] private ProjectSettings _projectSettings;

        private void OnEnable()
        {
            EventManager.TimerStop += AnnounceWinner;
        }
        
        private void OnDisable()
        {
            EventManager.TimerStop -= AnnounceWinner;
        }
        
        private  void Awake()
        {
            _players = new Dictionary<string, GameObject>();
        }
        
        
        private void AnnounceWinner()
        {
            var localScore = _scoreManager.GetScore();
            
            foreach (var player in _players)
            {
                if (player.Key != _localUser.SessionId)
                {
                    var remoteScore = player.Value.GetComponent<PlayerNetworkRemoteSync>().Score;
                    if (remoteScore <= localScore) continue;
                    _storage.Data.ScoreData.LoseCount++;
                    _isWin = false;
                    EventManager.OnMatchEnd();
                    return;
                }
            }

            _isWin = true;
            _storage.Data.ScoreData.WinCount++;
            EventManager.OnMatchEnd();
        }

        private void SpawnPlayerGamePanel(string matchId,IUserPresence user,Transform parent)
        {
            if (_players.ContainsKey(user.SessionId))
            {
                return;
            }
            
            var isLocal = user.SessionId == _localUser.SessionId;
            // Choose the appropriate player prefab based on if it's the local player or not.
            var player = isLocal ? _playerPanelFactory.Create(_projectSettings.LocalUserPanel) : _playerPanelFactory.Create(_projectSettings.RemoteUserPanel);
            player.transform.SetParent(parent);
            player.GetComponent<RectTransform>().localScale = Vector3.one;
            
            if (!isLocal)
            {
                player.GetComponent<PlayerNetworkRemoteSync>().NetworkData = new RemotePlayerNetworkData
                {
                    MatchId = matchId,
                    User = user,
                };
                
            }
            
            _players.Add(user.SessionId,player.gameObject);
        }

        private void DestroyPlayers()
        {
            foreach (var player in _players.Values)
            {
                Destroy(player);
            }
            
            _players.Clear();
        }
        
    }
}
