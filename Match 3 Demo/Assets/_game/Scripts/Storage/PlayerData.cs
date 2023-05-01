using System;
using UnityEngine;

namespace _game.Scripts.Storage
{
   [Serializable]
   public class PlayerData
   {
      public string UserName;
      public Gender Gender;
      public bool IsRegistered;
   }

   public enum Gender
   {
      Boy,
      Girl,
   }
}
