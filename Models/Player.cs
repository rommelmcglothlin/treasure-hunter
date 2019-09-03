using System;
using System.Collections.Generic;
using TreasureHunter.Interfaces;

namespace TreasureHunter.Models
{
  public class Player : IPlayer
  {
    public string Name { get; set; }

    public int Score { get; set; } = 0;
    public int Morale { get; set; } = 20;
    public List<IItem> Inventory { get; set; }
    public int MovesRemaining { get; set; } = 20;

    public Player(string name)
    {
      Name = name;
      Inventory = new List<IItem>();
    }
  }
}