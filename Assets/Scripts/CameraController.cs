using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float cameraSpeed = 2;
    public float scrollSensitivity = 1f;
    public float cameraSize = 10f;
    public float minSize = 3f;
    public float maxSize = 20f;

    private Camera camera;


	void Start()
    {
        camera = GetComponent<Camera>();
    }

    
    void Update()
    {
        MoveTarget();
        CameraSize();
    }

    private void MoveTarget()
    {
		transform.position = (Vector3)Vector2.Lerp(transform.position, Target.position, cameraSpeed * Time.deltaTime) + new Vector3(0, 0, -10);
	}

	private void CameraSize()
	{
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        cameraSize -= mouseWheel * scrollSensitivity * Time.deltaTime;
        if (cameraSize < minSize)
            cameraSize = minSize;
        else if(cameraSize > maxSize)
            cameraSize = maxSize;
		camera.orthographicSize = cameraSize;
	}
}
