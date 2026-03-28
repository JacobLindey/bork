using Godot;

public partial class TileBag : Node
{
    [Export]
    private LootTable LootTable;

    public TileType Next => LootTable.GetNextEntry<TileType>();
}