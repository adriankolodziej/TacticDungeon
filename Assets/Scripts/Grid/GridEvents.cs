using UnityEngine.Events;

public class GridEvents
{
    public static System.EventHandler<Grid> OnGridCreated;
    public static System.EventHandler<(int, int)> OnCellChosen;
}

