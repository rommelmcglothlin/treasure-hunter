using System.Collections.Generic;

namespace TreasureHunter.Interfaces
{
  public interface IBoundary
  {
    string Name { get; set; }
    string Description { get; set; }

    bool HRinRoom { set; get; }
    List<IItem> Items { get; set; }
    Dictionary<string, IBoundary> NeighborBoundaries { get; set; }

    // string AltDescription { get; set; } //NOTE you might not use this but could be useful for extension ideas
    // bool IsLosable { get; set; } //NOTE you might not use this but could be useful for extension ideas

    void AddNeighborBoundary(IBoundary neighbor, bool autoAdd);
  }
}