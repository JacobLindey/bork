using Godot;

public partial class PuzzleLevel : Node2D
{
    [Export]
    public Grid Grid { get; private set; }
    
    [Export]
    public TileBag TileBag { get; private set; }

    [Export]
    private PackedScene TileFactory;

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
            tile.SetType(TileBag.Next);
            cell.SetTile(tile);
        }
    }
}
