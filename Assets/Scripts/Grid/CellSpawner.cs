using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubeCellPrefab;

    private List<Cell> cellGrid;

    void OnEnable()
    {
        GridEvents.OnGridCreated += SpawnCells;
    }
    void Start()
    {

    }

    private void SpawnCells(object sender, Grid<PathNode> grid)
    {
        cellGrid = new List<Cell>();
        for (int x = 0; x < grid.GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GridArray.GetLength(1); y++)
            {
                var newCell = Instantiate(cubeCellPrefab, grid.GetWorldPosition(x, y), Quaternion.identity, this.transform);
                Cell cell = newCell.GetComponent<Cell>();
                cell.SetCoordinates(x, y);
                cellGrid.Add(cell);
            }
        }
    }

    void OnDisable()
    {
        GridEvents.OnGridCreated -= SpawnCells;
    }
}
