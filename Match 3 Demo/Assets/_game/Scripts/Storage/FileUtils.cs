using System;
using System.IO;
using UnityEngine;

namespace _game.Scripts.Storage
{
    public static class FileUtils
    {
        public static bool Read(string filename, out string content)
        {
            var path = Path.Combine(Application.persistentDataPath, filename);

            if (!File.Exists(path))
            {
                content = "";
                return false;
            }

            try
            {
                content = File.ReadAllText(path);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            content = "";
            return false;
        }
        
        public static bool Write(string filename, string content)
        {
            var path = Path.Combine(Application.persistentDataPath, filename);

            try
            {
                File.WriteAllText(path, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return false;
        }

        public static void Delete(string filename)
        {
            var path = Path.Combine(Application.persistentDataPath, filename);

            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        } 
    }
}