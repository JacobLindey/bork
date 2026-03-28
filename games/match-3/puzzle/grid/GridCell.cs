using System;
using System.Collections.Generic;
using Godot;

public partial class GridCell : Node2D
{
    [Export]
    private Sprite2D SelectionSprite { get; set; }

    [Export]
    private Node2D TileContainer;
    
    public CellState State { get; set; }

    public Tile ContainedTile => TileContainer.GetChildOrNull<Tile>(0);
    
    public Vector2I Coordinates { get; private set; }

    public bool HasTile => ContainedTile is not null;

    public void Init(Vector2I coordinates)
    {
        Coordinates = coordinates;
    }
    
    public void SetTile(Tile tile)
    {
        tile.Reparent(TileContainer);
        tile.QueuePosition(GlobalPosition);
    }

    public void RemoveTile()
    {
        if (!HasTile)
        {
            return;
        }

        var temp = ContainedTile;
        temp.Reparent(this);
        temp.QueueFree();
    }

    public bool TryGetTile(out Tile tile)
    {
        if (HasTile)
        {
            tile = ContainedTile;
            return true;
        }

        tile = null;
        return false;
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

    public void SetHover(bool value)
    {
        if (value)
        {
            State |= CellState.Hover;
        }
        else
        {
            State &= ~CellState.Hover;
        }
    }

    public void OnSelectionAreaMouseEntered()
    {
        SetHover(true);
    }

    public void OnSelectionAreaMouseExited()
    {
        SetHover(false);
    }
}

[Flags]
public enum CellState
{
    None = 0,
    Hover = 1 << 0,
    Selected = 1 << 1
}
