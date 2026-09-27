using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize = 1;
    private int[,] gridArray;

    public int[,] GridArray { get { return gridArray; } }
    public float CellSize { get { return cellSize; } }

    public Grid(int width, int height, float cellSize = 1)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
}
