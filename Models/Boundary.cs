using System;
using System.Collections.Generic;
using TreasureHunter.Interfaces;

namespace TreasureHunter.Models
{
  public class Boundary : IBoundary
  {
    public string Name { get; set; }
    public string Description { get; set; }

    public List<IItem> Items { get; set; }
    public Dictionary<string, IBoundary> NeighborBoundaries { get; set; }

    public bool HRinRoom { get; set; } = false;

    public void AddNeighborBoundary(IBoundary neighbor, bool autoAdd)
    {
      NeighborBoundaries.Add(neighbor.Name, neighbor);
      if (autoAdd)
      {
        neighbor.AddNeighborBoundary(this, false);
      }
    }
    public Boundary(string name, string description)
    {
      Name = name;
      Description = description;
      HRinRoom = false;
      Items = new List<IItem>();
      NeighborBoundaries = new Dictionary<string, IBoundary>();
    }




  }
}