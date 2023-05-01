using UnityEngine;
using Zenject;

namespace _game.Scripts.Installers
{
   public class PlayerPanelInstaller : MonoInstaller
   {
      public override void InstallBindings()
      {
         Container.BindFactory<Object, PlayerPanelController, PlayerPanelFactory>().FromFactory<PrefabFactory<PlayerPanelController>>();
      }
   }
}
