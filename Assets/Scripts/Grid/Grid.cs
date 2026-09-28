using System;
using UnityEngine;

public class Grid<TObject>
{
    private int width;
    private int height;
    private float cellSize = 1;
    private TObject[,] gridArray;

    public TObject[,] GridArray { get { return gridArray; } }
    public float CellSize { get { return cellSize; } }

    public Grid(int width, int height, float cellSize, Func<Grid<TObject>, int, int, TObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new TObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public TObject GetGridObject(int x, int y)
    {
        return GridArray[x, y];
    }

    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize;
    }
}
