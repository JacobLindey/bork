using System;
using Godot;

public partial class GridCell : Node2D
{
    [Export]
    private Sprite2D SelectionSprite { get; set; }

    [Export]
    private Node2D TileContainer;
    
    public CellState State { get; set; }
    
    public Tile ContainedTile { get; private set; }
    
    public Vector2I Coordinates { get; private set; }

    public bool HasTile => ContainedTile is not null;

    public void Init(Vector2I coordinates)
    {
        Coordinates = coordinates;
    }
    
    public void SetTile(Tile tile)
    {
        tile.Reparent(TileContainer);
        ContainedTile = tile;
        tile.QueuePosition(GlobalPosition);
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        SelectionSprite.Visible = State != CellState.None;
    }
    
    public void OnSelectionAreaInputEvent(Node viewPort, InputEvent e, int shapeIdx)
    {
        
        if (e is InputEventMouseButton {Pressed: true})
        {
            CellEventHandler.OnCellPressed(this);
        }
    }

    public void SetSelected(bool value)
    {
        if (value)
        {
            State |= CellState.Selected;
        }
        else
        {
            State &= ~CellState.Selected;
        }
    }

    public void OnSelectionAreaMouseEntered()
    {
        State |= CellState.Hover;
    }

    public void OnSelectionAreaMouseExited()
    {
        State &= ~CellState.Hover;
    }
}

[Flags]
public enum CellState
{
    None = 0,
    Hover = 1 << 0,
    Selected = 1 << 1
}
