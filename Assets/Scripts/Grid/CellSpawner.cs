using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubeCellPrefab;

    void OnEnable()
    {
        GridEvents.OnGridCreated += SpawnCells;
    }
    void Start()
    {
        
    }

    private void SpawnCells(object sender, Grid<PathNode> grid)
    {
        for(int x = 0; x < grid.GridArray.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GridArray.GetLength(1); y++)
            {
                var newCell = Instantiate(cubeCellPrefab, grid.GetWorldPosition(x, y), Quaternion.identity, this.transform);
                Cell cell = newCell.GetComponent<Cell>();
                cell.SetCoordinates(x, y);
                
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.green, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.green, 100f);
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(0, grid.GridArray.GetLength(1)), grid.GetWorldPosition(grid.GridArray.GetLength(0), grid.GridArray.GetLength(1)), Color.green, 100f);
        Debug.DrawLine(grid.GetWorldPosition(grid.GridArray.GetLength(0), 0), grid.GetWorldPosition(grid.GridArray.GetLength(0), grid.GridArray.GetLength(1)), Color.green, 100f);
    }

    void OnDisable()
    {
        GridEvents.OnGridCreated -= SpawnCells;
    }
}
