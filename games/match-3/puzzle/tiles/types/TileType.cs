using Godot;

public abstract partial class TileType : Resource
{
    [Export]
    public virtual string Name { get; set; }
    
    [Export]
    public virtual string Description { get; set; }
    
    [Export]
    public Texture2D Texture { get; set; }
}