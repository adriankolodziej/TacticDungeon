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
        Grid grid = new Grid(10, 10, 1.01f);
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
