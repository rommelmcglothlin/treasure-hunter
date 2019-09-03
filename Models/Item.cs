using System;
using System.Collections.Generic;
using TreasureHunter.Interfaces;

namespace TreasureHunter.Models
{
  public class Item : IItem
  {
    public string Name { get; set; }
    public int AmmoCount { get; set; }
    public string Description { get; set; }

    public Item(string name, int ammoCount = 0, string description = "For Cricket use")
    {
      Name = name;
      Description = description;
      AmmoCount = ammoCount;
    }
  }
}