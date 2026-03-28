using Godot;

public partial class TileBag : Node
{
    [Export]
    private LootTable LootTable;

    [Export]
    private TileType DefaultType;

    [Export]
    private PackedScene TileFactory;

    public Tile GetNext()
    {
        var tile = TileFactory.Instantiate<Tile>();
        tile.SetType(DefaultType);
     
        var type = LootTable.GetNextEntry<TileType>();
        tile.SetType(type);
        return tile;
    }
}