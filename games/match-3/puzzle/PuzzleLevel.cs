using Godot;

public partial class PuzzleLevel : Node2D
{
    [Export]
    public Grid Grid { get; private set; }

    [Export]
    private PackedScene TileFactory;

    [Export]
    private TileType TileType;

    public override void _Ready()
    {
        base._Ready();
        
        Grid.CreateGrid(new Vector2I(6, 6));
        
        ProcessGrid();
    }

    private void ProcessGrid()
    {
        foreach (var cell in Grid.EmptyCells)
        {
            var tile = TileFactory.Instantiate<Tile>();
            tile.SetType(TileType);
            cell.SetTile(tile);
        }
    }
}
