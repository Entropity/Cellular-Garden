using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelPerfectCamera))]
public class CameraController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;

    [Header("Zoom Settings")]
    [SerializeField] private int minPPU = 16;
    [SerializeField] private int maxPPU = 64;
    [SerializeField] private int zoomStep = 4;
    [SerializeField] private float zoomSmoothTime = 0.2f;
    [SerializeField] private float mouseZoomSensitivity = 0.5f;

    private PixelPerfectCamera pixelPerfectCamera;
    private Vector2 movementInput;
    private Vector2 currentVelocity;
    private int targetPPU;
    private float zoomVelocity;

    private void Awake() {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        targetPPU = pixelPerfectCamera.assetsPPU;
    }

    private void Update() {
        HandleMovement();
        HandleZoom();
        HandleMouseZoom();
    }

    private void HandleMovement() {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        Vector2 targetVelocity = input * moveSpeed;

        // Apply acceleration/deceleration
        if (input.magnitude > 0.1f) {
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        } else {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        // Apply movement
        Vector3 movement = new Vector3(currentVelocity.x, currentVelocity.y, 0f) * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void HandleZoom() {
        // Keyboard zoom
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus)) {
            targetPPU = Mathf.Min(targetPPU + zoomStep, maxPPU);
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) {
            targetPPU = Mathf.Max(targetPPU - zoomStep, minPPU);
        }

        // Smooth zoom
        if (pixelPerfectCamera.assetsPPU != targetPPU) {
            float smoothedPPU = Mathf.SmoothDamp(pixelPerfectCamera.assetsPPU, targetPPU, ref zoomVelocity, zoomSmoothTime);
            pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(smoothedPPU);
        }
    }

    private void HandleMouseZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            // Adjust zoom step based on sensitivity
            int scrollZoomStep = Mathf.RoundToInt(zoomStep * Mathf.Abs(scroll) * mouseZoomSensitivity);
            scrollZoomStep = Mathf.Max(1, scrollZoomStep);

            if (scroll > 0) {
                targetPPU = Mathf.Min(targetPPU + scrollZoomStep, maxPPU);
            } else {
                targetPPU = Mathf.Max(targetPPU - scrollZoomStep, minPPU);
            }
        }
    }
}