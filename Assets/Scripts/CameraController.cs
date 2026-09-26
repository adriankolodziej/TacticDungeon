using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    private InputSystem inputSystem;
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
    private Vector3 lastPosition;

    private Vector3 startDrag;

    void Awake()
    {
        inputSystem = new InputSystem();
        if (cameraTransform == null)
        {
            cameraTransform = this.GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        GetKeyboardMovement();
        UpdateVelocity();
        UpdateBasePosition();
    }


    void OnEnable()
    {
        cameraTransform.LookAt(this.transform);
        lastPosition = this.transform.position;
        cameraMovement = inputSystem.Camera.MoveCamera;

        inputSystem.Camera.Enable();
    }

    void OnDisable()
    {
        inputSystem.Camera.Disable();
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
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = this.transform.position;
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
        if(targetPosition.sqrMagnitude > 0.1f)
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
}
