
using _game.Scripts.Storage;
using UnityEditor;
using UnityEngine;


public class MenuItems
{
   [MenuItem("Storage/Clear Storage", false, 1)]
   private static void ClearStorage()
   {
      var jsonStorage = new JsonStorage();
      jsonStorage.Clear();
      
      PlayerPrefs.DeleteAll();
      
   }
}
