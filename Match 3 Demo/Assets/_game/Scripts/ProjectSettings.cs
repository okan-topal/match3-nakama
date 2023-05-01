using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts
{
    [CreateAssetMenu(fileName = "Project Settings",menuName = "Settings/"+ nameof(ProjectSettings),order =0)]
    public class ProjectSettings : ScriptableObject
    {
        [Title("User")] 
        public PlayerNetworkLocalSync LocalUserPanel;
        public PlayerNetworkRemoteSync RemoteUserPanel;
        public Sprite BoyAvatar;
        public Sprite GirlAvatar;
        
        [Title("Board")]
        public int RowCount;
        public int ColumnCount;
        public Tile Tile;
        public Gem[] Gems;

        [Title("Timer")]
        public float TimerCountDown;
        
    }
}
