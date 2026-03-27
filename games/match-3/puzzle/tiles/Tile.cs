using Godot;

public partial class Tile : Node2D
{
    [Export]
    private Sprite2D Sprite;
    
    public void QueuePosition(Vector2 position)
    {
        SetGlobalPosition(position);
    }

    public void SetType(TileType type)
    {
        Sprite.Texture = type.Texture;
    }
}