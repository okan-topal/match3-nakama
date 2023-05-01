using _game.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace _game.Scripts.Installers
{
   public class GemInstaller : MonoInstaller
   {
      public override void InstallBindings()
      {
         Container.BindFactory<Object, Gem, GemFactory>().FromFactory < PrefabFactory<Gem>>();
      }
   }
}
