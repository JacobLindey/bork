using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class TileMatcher : Node
{
    public bool TryMatch(Grid grid, out TileMatch[] matches)
    {
        List<GridCell> checkedCells = [];
        List<TileMatch> foundMatches = [];
        foreach (var cell in grid.Enumerate())
        {
            if (checkedCells.Contains(cell))
            {
                continue;
            }
            checkedCells.Add(cell);
            
            var hasUpNeighbor = grid.TryGetCell(cell.Coordinates + Vector2I.Up, out var upNeighbor);
            var hasDownNeighbor = grid.TryGetCell(cell.Coordinates + Vector2I.Down, out var downNeighbor);

            if (
                hasUpNeighbor &&
                hasDownNeighbor &&
                cell.TryGetTile(out var tile) &&
                upNeighbor.TryGetTile(out var upTile) &&
                downNeighbor.TryGetTile(out var downTile) &&
                tile.Type.Name == upTile.Type.Name &&
                tile.Type.Name == downTile.Type.Name)
            {
                GD.Print("!!found match!!");
                GD.Print($"\t{upNeighbor.Coordinates} | {upTile.Type.Name}");
                GD.Print($"\t{cell.Coordinates} | {cell.ContainedTile.Type.Name}");
                GD.Print($"\t{downNeighbor.Coordinates} | {downTile.Type.Name}");
                foundMatches.Add(new TileMatch([upNeighbor, cell, downNeighbor]));
            }
        }

        matches = foundMatches.ToArray();
        return matches.Length != 0;
    }
}

public class TileMatch
{
    public TileMatch(IEnumerable<GridCell> cells)
    {
        Cells = cells.ToList();
    }

    public List<GridCell> Cells { get; }
}
