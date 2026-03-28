using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PuzzleLevel : Node2D
{
    [Export] private Grid Grid;
    [Export] private TileBag TileBag;
    [Export] private TileMatcher TileMatcher;



    public override void _Ready()
    {
        base._Ready();
        
        Grid.CreateGrid(new Vector2I(6, 6));
        Grid.Changed += OnGridChanged;
    }

    private void OnGridChanged(GridChangedEventArgs args)
    {
        
    }
    
    private void ProcessMatches()
    {
        if (TileMatcher.TryMatch(Grid, out var matches))
        {
            foreach (var match in matches)
            {
                GD.Print($"score {match.Cells.Select(x => x.Coordinates).CollectionString()}");
                Score(match);
            }
        }
    }

    private void CheckMatches()
    {
        if (TileMatcher.TryMatch(Grid, out var matches))
        {
            foreach (var match in matches)
            {
                GD.Print($"found match {match.Cells.Select(x => x.Coordinates).CollectionString()}");
                foreach (var cell in match.Cells)
                {
                    cell.SetHover(true);
                }
            }
        }
    }

    private void ClearHighlight()
    {
        foreach (var cell in Grid.Enumerate())
        {
            cell.SetHover(false);
        }
    }

    private void FillGrid()
    {
        foreach (var cell in Grid.EmptyCells)
        {
            PopulateCell(cell);
        }
    }

    private void ApplyGravity()
    {
        for (var col = 0; col < Grid.Columns; col++)
        {
            for (var row = Grid.Rows-1; row >= 0; row--)
            {
                var checkedCoords = new Vector2I(col, row);
                if (Grid.GetCell(checkedCoords).HasTile)
                {
                    continue;
                }

                for (var ri = row - 1; ri >= 0; ri--)
                {
                    var c = new Vector2I(col, ri);
                    if (Grid.GetCell(c).HasTile)
                    {
                        Grid.Swap(checkedCoords, c);
                    }
                }
            }
        }
    }

    private void Score(TileMatch match)
    {
        foreach (var cell in match.Cells)
        {
            cell.RemoveTile();
        }
    }

    private void PopulateCell(GridCell cell)
    {
        var tile = TileBag.GetNext();
        AddChild(tile);
        cell.SetTile(tile);
    }
}

public static class ListExtensions
{
    public static string CollectionString<T>(this IEnumerable<T> e)
    {
        return "[" + string.Join(", ", e) + "]";
    }
}
