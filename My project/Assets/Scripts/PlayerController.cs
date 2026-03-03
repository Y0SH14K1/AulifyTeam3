using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 200f; // tweak this in Inspector
    public Transform cameraTransform;      // assign your child camera here

    private CharacterController controller;
    private float yVelocity;
    private float xRotation = 0f;
    private bool isUiVisible = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            // automatically find child camera if not assigned
            Camera cam = GetComponentInChildren<Camera>();
            if (cam != null) cameraTransform = cam.transform;
            else Debug.LogError("PlayerController: No Camera found!");
        }

        // Set initial cursor state
        SetCursorState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isUiVisible = !isUiVisible;
            SetCursorState(isUiVisible);
        }

        if (!isUiVisible)
        {
            HandleMouseLook();
        }
        
        HandleMovement();
    }

    void SetCursorState(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }

    void HandleMouseLook()
    {
        // Get mouse input from old Input system
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        // Get WASD input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (controller.isGrounded && yVelocity < 0f)
        {
            yVelocity = -2f; // small downward force to stay grounded
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed + Vector3.up * yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}