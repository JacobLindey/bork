using Godot;

public abstract partial class TileType : Resource
{
    [Export]
    public string Name { get; private set; }
    
    [Export]
    public string Description { get; private set; }
    
    [Export]
    public Texture2D Texture { get; private set; }
}