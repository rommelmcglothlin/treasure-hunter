using System.Collections.Generic;

namespace TreasureHunter.Interfaces
{
  public interface IPlayer
  {
    string Name { get; set; }

    int Score { get; set; }

    int Morale { get; set; }

    int MovesRemaining { get; set; }

    List<IItem> Inventory { get; set; }
    // int HealthPoints { get; set; } //NOTE you might not use this but could be useful for extension ideas
  }
}