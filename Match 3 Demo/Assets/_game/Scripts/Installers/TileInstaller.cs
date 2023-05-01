using _game.Scripts.Factories;
using UnityEngine;
using Zenject;

public class TileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<Object, Tile, TileFactory>().FromFactory < PrefabFactory<Tile>>();
    }
}
