using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

[Serializable]
[CreateAssetMenu]
public class NakamaConnection : ScriptableObject
{
    public string Scheme = "http";
    public string Host = "localhost";
    public int Port = 7350;
    public string ServerKey = "defaultkey";
    private const string SessionPrefName = "nakama.session";
    private const string DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";
   
    public IClient Client;
    public ISession Session;
    public ISocket Socket;

    private string currentMatchmakingTicket;
    private string currentMatchId;
    
    public async Task Connect()
    {
        Client = new Nakama.Client(Scheme, Host, Port, ServerKey, UnityWebRequestAdapter.Instance);

        var authToken = PlayerPrefs.GetString(SessionPrefName);
        if (!string.IsNullOrEmpty(authToken))
        {
            var session = Nakama.Session.Restore(authToken);
            if (!session.IsExpired)
            {
                Session = session;
            }
        }

        if (Session == null)
        {
            string deviceId;
            if (PlayerPrefs.HasKey(DeviceIdentifierPrefName))
            {
                deviceId = PlayerPrefs.GetString(DeviceIdentifierPrefName);
            }
            else
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                if (deviceId == SystemInfo.unsupportedIdentifier)
                {
                    deviceId = System.Guid.NewGuid().ToString();
                }

                PlayerPrefs.SetString(DeviceIdentifierPrefName, deviceId);
            }
            
            Session = await Client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
            PlayerPrefs.SetString(SessionPrefName,Session.AuthToken);
        }
        
        Socket = Client.NewSocket();
        await Socket.ConnectAsync(Session, true);
    }

    public async Task FindMatch(int minPlayers = 2)
    {
        var matchmakingProperties = new Dictionary<string, string>
        {
            { "engine", "unity" }
        };

        var matchmakerTicket =
            await Socket.AddMatchmakerAsync("+properties.engine:unity", minPlayers, 2, matchmakingProperties);
        currentMatchmakingTicket = matchmakerTicket.Ticket;
        Debug.Log("Finding Match..");
    }

    public async Task CancelMatchMaking()
    {
        await Socket.RemoveMatchmakerAsync(currentMatchmakingTicket);
    }
}
