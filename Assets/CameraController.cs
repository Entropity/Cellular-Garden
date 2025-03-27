using UnityEngine;

public class CameraController : MonoBehaviour {
    public float moveSpeed = 5f; // Speed of camera movement
    public float zoomSpeed = 5f; // Speed of camera zooming
    private float minZoom = 0.05f;   // Minimum orthographic size
    public float maxZoom = 10f; // Maximum orthographic size
    public float speed = 1;
    private Camera cam;
    
    void Start() {
        // Get the Camera component
        cam = GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("No Camera component found on this GameObject.");
        }
    }

    void Update() {
        // Handle camera movement
        MoveCamera();

        // Handle camera zoom
        ZoomCamera();
    }

    void MoveCamera() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 targetPosition = cam.transform.position + new Vector3(horizontal, vertical, 0.0f) * speed * cam.orthographicSize * Time.deltaTime;
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, 0.1f);
    }

    void ZoomCamera() {
        // Zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            float targetSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, 0.1f);
        }
    }
}