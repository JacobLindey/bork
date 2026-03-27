using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Grid : Node2D
{
    private GridCell[] _grid = [];
    private uint _columns;
    private uint _rows;
    
    private readonly Stack<GridCell> _selectionStack = new();
    private List<GridCell> _availableNeighbors = [];
    
    public int Columns { get; private set; }
    public int Rows { get; private set; }
    
    [Export]
    public Vector2 CellSize { get; private set; }
    
    [Export]
    public PackedScene CellFactory { get; private set; }

    public IEnumerable<GridCell> EmptyCells => _grid.Where(x => !x.HasTile);
    
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
                _grid[x * Rows + y] = CreateCell(x, y);
            }
        }
    }

    private GridCell CreateCell(int x, int y)
    {
        var cell = CellFactory.Instantiate<GridCell>();
        var position = GetLocalPosition(x, y);
        AddChild(cell);
        cell.Position = position;
        cell.Owner = this;
        return cell;
    }

    private Vector2 GetLocalPosition(int x, int y)
    {
        var posX = CellSize.Y * x;
        var posY = CellSize.Y * y;
        return new Vector2(posX, posY);
    }
    
    private void OnCellPressed(GridCell cell)
    {
        if (_selectionStack.Count == 0)
        {
            _availableNeighbors = GetNeighbors(cell);
            cell.SetSelected(true);
            _selectionStack.Push(cell);
        }
        else
        {
            if (_selectionStack.Peek() == cell)
            {
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
        _availableNeighbors.Clear();
    }
    
    private List<GridCell> GetNeighbors(GridCell cell)
    {
        return [];
    }
}
