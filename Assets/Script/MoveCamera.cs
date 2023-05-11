using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity = 6f;
    [SerializeField] private Transform target;
    private float distanceFromTarget;
    [SerializeField] private float zoomSensitivity = 1f;

    private float rotationX;
    private float rotationY;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField] private float smoothTime = 0.3f;

    private void Start()
    {
        distanceFromTarget = GlobalVar.dimensions * 1.85f + 3.6f;
        updatePosition();
        #if UNITY_WEBGL && !UNITY_EDITOR  // The sensitivity is much higher on the WebGL build
                mouseSensitivity /= 3;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(InputSystem.keybinds["Rotate"])) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * -Mathf.Sign(Vector3.Dot(transform.up, Vector3.down));
            float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationX += mouseX;
            rotationY += mouseY;

        }

        Vector3 nextRotation = new Vector3(rotationY, rotationX);
        if (!compareVector3(currentRotation, nextRotation)) {
            currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
            transform.localEulerAngles = currentRotation;

            updatePosition();
        }

        // Zoom
        if(Input.GetAxis("Mouse ScrollWheel") != 0) {
            distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * getRelativeZoomSensitivity();
            distanceFromTarget = distanceFromTarget > 0f ? distanceFromTarget : 0;
            updatePosition();
        }
        
    }

    private float getRelativeZoomSensitivity()
    {
        // Based on regression from sample data
        float distance = Vector3.Distance(target.position, gameObject.transform.position);
        return (distance * 0.37f + 0.35f) * zoomSensitivity;
    }

    private bool compareVector3(Vector3 vectorA, Vector3 vectorB)
    {
        return Vector3.SqrMagnitude(vectorA - vectorB) < 0.0001;
    }

    private void updatePosition()
    {
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
