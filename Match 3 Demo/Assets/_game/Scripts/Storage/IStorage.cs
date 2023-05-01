using System;
using UnityEngine;

namespace _game.Scripts.Storage
{
    public abstract class  IStorage : IDisposable
    {
        public Data Data { get; protected set; }
        
        protected abstract void Save();
        protected abstract void Load();
        public abstract void Clear();

        protected IStorage()
        {
            Debug.Log("Storage Initialized...");
            Load();
        }

        public void Dispose()
        {
            Debug.Log("Storage Disposed...");
            Save();
        }
    }
}
