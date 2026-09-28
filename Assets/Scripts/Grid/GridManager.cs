using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private CustomInputSystem inputSystem;

    void Awake()
    {
        inputSystem = new CustomInputSystem();
    }
    
    void Start()
    {
        Grid<PathNode> grid = new Grid<PathNode>(10, 10, 1f, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
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
                }
            }
        }
    }

    void OnDestroy()
    {
        GridEvents.OnGridCreated = null;
    }
}
