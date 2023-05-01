using System;

namespace _game.Scripts.Storage
{
  [Serializable]
  public class Data
  {
    public PlayerData PlayerData = new PlayerData();
    public ScoreData ScoreData = new ScoreData();
    public CurrencyData CurrencyData = new CurrencyData();

  }
}
