public class CellEventHandler
{
    public delegate void CellPressedEventHandler(GridCell cell);
    
    public static event CellPressedEventHandler CellPressed;

    public static void OnCellPressed(GridCell cell)
    {
        CellPressed?.Invoke(cell);
    }
}