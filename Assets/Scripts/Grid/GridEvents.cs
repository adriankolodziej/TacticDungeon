using UnityEngine.Events;

public class GridEvents
{
    public static System.EventHandler<Grid<PathNode>> OnGridCreated;
    public static System.EventHandler<(int, int)> OnCellChosen;
    public static System.EventHandler<(int, int)> OnCellChosenForPath;
}

