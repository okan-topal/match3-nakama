using System.Linq;
using _game.Scripts.Storage;
using Zenject;

namespace _game.Scripts.Installers
{
   public class ProjectInstaller : MonoInstaller
   {
      public ProjectSettings ProjectSettings;
      public override void InstallBindings()
      {
         Container.Bind(new[] {typeof(IStorage)}.Concat(typeof(IStorage).GetInterfaces())).To<JsonStorage>()
            .AsSingle().NonLazy();
         Container.Bind<ProjectSettings>().FromNewScriptableObject(ProjectSettings).AsSingle();
      }
   }
}
