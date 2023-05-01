using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace _game.Scripts
{
    public partial class GameManager
    {
        public NakamaConnection NakamaConnection;
        private IDictionary<string, GameObject> _players;
        private IUserPresence _localUser;
        private IMatch _currentMatch;
        private async void Start()
        {
            var mainThread = UnityMainThreadDispatcher.Instance();
            await NakamaConnection.Connect();
      
            NakamaConnection.Socket.ReceivedMatchmakerMatched += m => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
            NakamaConnection.Socket.ReceivedMatchPresence += m => mainThread.Enqueue(() => OnReceivedMatchPresence(m));
        }
        
        private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matched)
        {
            _localUser = matched.Self.Presence;
            var match = await NakamaConnection.Socket.JoinMatchAsync(matched);
            _currentMatch = match;
            Debug.Log("Our ID: "+ match.Self.SessionId);
            foreach (var user in match.Presences)
            {
                Debug.Log("Connected ID: "+user.SessionId);
            }
            
            _panelController.Show(PanelType.Game);

            foreach (var user in match.Presences)
            {
                SpawnPlayerGamePanel(match.Id, user,_panelController.GetUserLayout());
            }
            EventManager.OnMatchStart();
            EventManager.OnTimerStart();
        }
        
        private void OnReceivedMatchPresence(IMatchPresenceEvent matchPresenceEvent)
        {
            // For each new user that joins, spawn a player for them.
            foreach (var user in matchPresenceEvent.Joins)
            {
                SpawnPlayerGamePanel(matchPresenceEvent.MatchId, user,_panelController.GetUserLayout());
       
            }
            // For each player that leaves, despawn their player.
            foreach (var user in matchPresenceEvent.Leaves)
            {
                if (_players.ContainsKey(user.SessionId))
                {
                    Destroy(_players[user.SessionId]);
                    _players.Remove(user.SessionId);
                }
            }
        }
        
        public void SendMatchInfo(long opCode, string state)
        { 
            NakamaConnection.Socket.SendMatchStateAsync(_currentMatch.Id, opCode, state);
        }
        
        public async Task QuitMatch()
        {
            await NakamaConnection.Socket.LeaveMatchAsync(_currentMatch);
            
            _currentMatch = null;
            _localUser = null;
            
            DestroyPlayers();
            EventManager.OnConnectionLost();

        }

    }
}
