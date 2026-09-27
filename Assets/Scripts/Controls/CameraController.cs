using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    private CustomInputSystem cameraInput;
    private InputAction cameraMovement;

    #region Horizontal Input
    [SerializeField]
    private float maxSpeed = 5f;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;
    [SerializeField]
    private float maxRotationSpeed = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float edgeTolerance = 0.1f;
    private float speed;
    #endregion
    #region Vertical Input
    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDamping = 10f;
    [SerializeField]
    private float minHeight = 3f;
    [SerializeField]
    private float maxHeight = 40f;
    [SerializeField]
    private float zoomSpeed = 10f;
    #endregion

    private Vector3 targetPosition;
    private float zoomHeight;
    private Vector3 horizontalVelocity;
    private Vector3 previousCameraPosition;

    private Vector3 startDrag;

    void Awake()
    {
        cameraInput = new CustomInputSystem();
        if (cameraTransform == null)
        {
            cameraTransform = this.GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        GetKeyboardMovement();
        CheckMouseAtScreenEdge();
        DragCamera();

        UpdateVelocity();
        UpdateBasePosition();
        UpdateCameraPosition();
    }


    void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        previousCameraPosition = this.transform.position;

        cameraMovement = cameraInput.Camera.MoveCamera;
        cameraInput.Camera.RotateCamera.performed += RotateCamera;
        cameraInput.Camera.ZoomCamera.performed += ZoomCamera;
        cameraInput.Camera.Enable();
    }

    void OnDisable()
    {
        cameraInput.Camera.RotateCamera.performed -= RotateCamera;
        cameraInput.Camera.ZoomCamera.performed -= ZoomCamera;
        cameraInput.Camera.Disable();
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - previousCameraPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        previousCameraPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = (cameraMovement.ReadValue<Vector2>().x * GetCameraRight() + (cameraMovement.ReadValue<Vector2>().y * GetCameraForward())).normalized;
        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputValue;
        }
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, acceleration * Time.deltaTime);
            this.transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, damping * Time.deltaTime);
            this.transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext context)
    {
        if (!Mouse.current.middleButton.isPressed)
        {
            return;
        }
        float inputValue = context.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext context)
    {
        float inputValue = -context.ReadValue<Vector2>().y / 9f;

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + inputValue * stepSize;

            if (zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            }
            else if (zoomHeight > maxHeight)
            {
                zoomHeight = maxHeight;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);

        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.down;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, zoomDamping * Time.deltaTime);
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
        {
            targetPosition += -GetCameraRight();
        }
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
        {
            targetPosition += GetCameraRight();
        }

        if (mousePosition.y < edgeTolerance * Screen.height)
        {
            targetPosition += -GetCameraForward();
        }
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
        {
            targetPosition += GetCameraForward();
        }

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            return;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startDrag = ray.GetPoint(distance);
            }
            else
            {
                targetPosition += startDrag - ray.GetPoint(distance);
            }
        }
    }
}
