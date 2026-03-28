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

            if (TryVerticalMatch(grid, cell, out var match))
            {
                foundMatches.Add(match);
            }

            if (TryHorizontalMatch(grid, cell, out match))
            {
                foundMatches.Add(match);
            }
        }

        matches = foundMatches.ToArray();
        return foundMatches.Count != 0;
    }

    private static bool TryVerticalMatch(Grid grid, GridCell cell, out TileMatch match)
    {
        return TryMatch(grid, cell, out match, Vector2I.Up, Vector2I.Down);
    }

    private static bool TryHorizontalMatch(Grid grid, GridCell cell, out TileMatch match)
    {
        return TryMatch(grid, cell, out match, Vector2I.Left, Vector2I.Right);
    }

    private static bool TryMatch(Grid grid, GridCell cell, out TileMatch match, params Vector2I[] relativeChecks)
    {
        match = null;
        if (!cell.HasTile)
        {
            return false;
        }

        var centerTile = cell.ContainedTile;
        List<GridCell> groupCells = [cell];
        foreach (var otherPos in relativeChecks)
        {
            if (!grid.TryGetCell(cell.Coordinates + otherPos, out var otherCell))
            {
                return false;
            }

            if (!otherCell.TryGetTile(out var otherTile))
            {
                return false;
            }

            if (centerTile.Type.Name != otherTile.Type.Name)
            {
                return false;
            }

            groupCells.Add(otherCell);
        }

        match = new TileMatch(groupCells);
        return true;
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
