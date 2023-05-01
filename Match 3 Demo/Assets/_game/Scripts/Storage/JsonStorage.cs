using Newtonsoft.Json;
using UnityEngine;

namespace _game.Scripts.Storage
{
    public sealed class JsonStorage : IStorage
    {
#if UNITY_EDITOR
        private const string FILENAME_DATA = "editor_storage.dat";

      #else
        private const string FILENAME_DATA = "device_storage.dat";
#endif
        protected override void Save()
        {
            var isSuccessful = FileUtils.Write(FILENAME_DATA, JsonConvert.SerializeObject(Data));

            if (!isSuccessful)
            {
                FileUtils.Write(FILENAME_DATA, JsonConvert.SerializeObject(Data));
            }
        }

        protected override void Load()
        {
            var isSuccessful = FileUtils.Read(FILENAME_DATA, out var rawJson);

            if (isSuccessful)
            {
                var settings = new JsonSerializerSettings {
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };

                Data = JsonConvert.DeserializeObject<Data>(rawJson, settings);
            }
            else if (Data == null)
            {
                Data = new Data();
            }
        }

        public override void Clear()
        {
            FileUtils.Delete(FILENAME_DATA);
            Data = new Data();
        }
    }
}