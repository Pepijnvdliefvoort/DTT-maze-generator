using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour {
    [Header("Zoom settings")]
    [SerializeField]
    private float zoomSpeed = 0.5f;
    [SerializeField]
    private float smoothingSpeed = 10.0f;
    [SerializeField]
    private float minimumZoomSize = 1f;
    [SerializeField]
    private float maximumZoomSize = 15.0f;

    private float zoomGoal;
    /// <summary>
    /// The orthographic zoom the camera has to smoothly lerp to
    /// </summary>
    private float ZoomGoal {
        get {
            return zoomGoal;
        }
        set {
            zoomGoal = Mathf.Clamp(value, minimumZoomSize, maximumZoomSize);
        }
    }

    private Camera m_camera;

    private void Awake() {
        m_camera = GetComponent<Camera>();
        zoomGoal = m_camera.orthographicSize;
    }

    private void Update() {
        m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, zoomGoal, Mathf.Min(smoothingSpeed * Time.deltaTime, 1f));
    }

    public void OnZoom(InputAction.CallbackContext context) {
        // Read the value from the scroll wheel
        float scrollAmount = -Mathf.Clamp(context.ReadValue<float>(), -1f, 1f);

        // Set the new zoom value of the camera
        ZoomGoal += scrollAmount * zoomSpeed;
    }
}