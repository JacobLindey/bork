using Godot;

[GlobalClass]
public partial class LootTableEntry : Resource
{
    [Export]
    public Resource Resource { get; set; }

    [Export]
    public int Weight { get; set; }
}