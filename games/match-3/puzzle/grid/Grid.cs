using System.Collections.Generic;
using System.Linq;
using Godot;

public delegate void GridChangedEventHandler(GridChangedEventArgs args);

public class GridChangedEventArgs
{
    
}

public partial class Grid : Node2D
{
    private GridCell[] _grid = [];
    private uint _columns;
    private uint _rows;
    
    private readonly List<GridCell> _selectionStack = new();
 
    public int Columns { get; private set; }
    public int Rows { get; private set; }
    
    [Export]
    public Vector2 CellSize { get; private set; }
    
    [Export]
    public PackedScene CellFactory { get; private set; }
    
    public IEnumerable<GridCell> EmptyCells => _grid.Where(x => !x.HasTile);

    public event GridChangedEventHandler Changed;
    
    public IEnumerable<GridCell> Enumerate()
    {
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var coords = new Vector2I(x, y);
                yield return GetCell(coords);
            }
        }
    }
    
    
    public override void _Ready()
    {
        base._Ready();
        CellEventHandler.CellPressed += OnCellPressed;
    }

    private Vector2I _gridSize;
    private Rect2 _gridArea;
    
    public void CreateGrid(Vector2I gridSize)
    {
        _gridSize = gridSize;
        _gridArea = new Rect2(Vector2.Zero, gridSize);
        Columns = gridSize.X;
        Rows = gridSize.Y;

        _grid = new GridCell[Columns * Rows];
        for (var x = 0; x < Columns; x++)
        {
            for (var y = 0; y < Rows; y++)
            {
                var coords = new Vector2I(x, y);
                SetCell(coords, CreateCell(coords));
            }
        }

        OnGridChanged();
    }

    public bool TryGetCell(Vector2I coords, out GridCell cell)
    {
        if (HasCell(coords))
        {
            cell = GetCell(coords);
            return true;
        }

        cell = null;
        return false;
    }
    
    private GridCell CreateCell(Vector2I coords)
    {
        var cell = CellFactory.Instantiate<GridCell>();
        var position = GetLocalPosition(coords);
        AddChild(cell);
        cell.Position = position;
        cell.Owner = this;
        cell.Init(coords);
        return cell;
    }

    private Vector2 GetLocalPosition(Vector2I coords)
    {
        var posX = CellSize.X * coords.X;
        var posY = CellSize.Y * coords.Y;
        return new Vector2(posX, posY);
    }
    
    private void OnCellPressed(GridCell cell)
    {
        if (_selectionStack.Count == 0)
        {
            cell.SetSelected(true);
            _selectionStack.Add(cell);
        }
        else
        {
            if (
                _selectionStack.Last() == cell || 
                !GetNeighbors(_selectionStack.Last()).Contains(cell))
            {
                ClearSelectionStack();
            }
            else
            {
                _selectionStack.Add(cell);
                Swap(_selectionStack[0], _selectionStack[1]);
                ClearSelectionStack();
            }
        }
    }

    private void ClearSelectionStack()
    {
        foreach (var cell in _selectionStack)
        {
            cell.SetSelected(false);
        }
        _selectionStack.Clear();
    }

    private readonly List<Vector2I> _directions =
    [
        Vector2I.Down,
        Vector2I.Left,
        Vector2I.Right,
        Vector2I.Up
    ];
    
    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = [];
        foreach (var direction in _directions)
        {
            var coordinates = cell.Coordinates + direction;
            if (HasCell(coordinates))
            {
                neighbors.Add(GetCell(coordinates));
            }
        }
        return neighbors;
    }

    public bool HasCell(Vector2I coords)
    {
        return
            coords.X >= 0 &&
            coords.X < Columns &&
            coords.Y >= 0 &&
            coords.Y < Rows;
    }

    public GridCell GetCell(Vector2I coords)
    {
        return _grid[GetIndex(coords)];
    }
    
    public void SetCell(Vector2I coords, GridCell cell)
    {
        _grid[GetIndex(coords)] = cell;
    }

    private int GetIndex(Vector2I coords)
    {
        return coords.X + coords.Y * Columns;
    }

    private void Swap(GridCell a, GridCell b)
    {
        GD.Print($"{a.Coordinates} <-> {b.Coordinates}");
        var temp = b.ContainedTile;
        b.SetTile(a.ContainedTile);
        a.SetTile(temp);
        OnGridChanged();
    }

    private void OnGridChanged()
    {
        Changed?.Invoke(new GridChangedEventArgs());
    }
}
