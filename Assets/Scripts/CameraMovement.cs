using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
	[Header("Sensitivity settings")]
	[SerializeField]
	private float sensitivityX = 1;
	[SerializeField]
	private float sensitivityY = 1;

	private Camera m_camera;

	private void Start() {
		m_camera = GetComponent<Camera>();
	}

	private void Update() {
		if (Input.GetMouseButton(0)) {
			m_camera.transform.position += -m_camera.transform.right * Input.GetAxis("Mouse X") * sensitivityX * m_camera.orthographicSize;
			m_camera.transform.position += -m_camera.transform.up * Input.GetAxis("Mouse Y") * sensitivityY * m_camera.orthographicSize;
		}

		switch(Input.mouseScrollDelta.y) {
			case -1:
				m_camera.orthographicSize += 10;
				break;
			case 1:
				m_camera.orthographicSize -= 10;
				break;
		}

		m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize, 10, 100);
	}
}
