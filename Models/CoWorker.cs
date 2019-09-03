using System;
using System.Collections.Generic;
using TreasureHunter.Interfaces;

namespace TreasureHunter.Models
{
  public class CoWorker : IPlayer
  {
    public string Name { get; set; }

    public int Score { set; get; } = 0;

    public int Morale { set; get; }

    public int MovesRemaining { set; get; }
    public List<IItem> Inventory { get; set; }

    public CoWorker(string name)
    {
      Name = name;
    }

  }


}
