using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private CustomInputSystem inputSystem;

    private Pathfinding pathfinding;
    void Awake()
    {
        inputSystem = new CustomInputSystem();
    }

    void Start()
    {
        Grid<PathNode> grid = new Grid<PathNode>(10, 10, 1f, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        pathfinding = new Pathfinding(10, 10);
        GridEvents.OnGridCreated.Invoke(this, grid);
    }

    void OnEnable()
    {
        inputSystem.Controls.Enable();
    }

    void OnDisable()
    {
        inputSystem.Controls.Disable();
    }

    void Update()
    {
        if (inputSystem.Controls.PointAction.WasPerformedThisFrame())
        {
            Debug.Log("Mouse clicked at position: " + Mouse.current.position.ReadValue());
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Cell cell = hit.collider.GetComponent<Cell>();
                if (cell != null)
                {
                    GridEvents.OnCellChosen?.Invoke(this, (cell.X, cell.Y));
                    Debug.Log("Clicked on cell at coordinates: " + cell.transform.position + " (x: " + cell.X + ", y: " + cell.Y + ")");
                    List<PathNode> path = pathfinding.FindPath(0, 0, cell.X, cell.Y);
                    for (int i = 0; i < path.Count; i++)
                    {
                        GridEvents.OnCellChosenForPath?.Invoke(this, (path[i].X, path[i].Y));
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        GridEvents.OnGridCreated = null;
    }
}
