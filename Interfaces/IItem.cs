namespace TreasureHunter.Interfaces
{
    public interface IItem
    {
        string Name { get; set; }
        string Description { get; set; }
        // int HPModifier { get; set; } //NOTE you might not use this but could be useful for extension ideas
    }
}