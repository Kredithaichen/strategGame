using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private float turnSpeed = 10.0f;
    [SerializeField]
    private float moveSpeed = 10.0f;

    // Use this for initialization
    void Start()
    {
        if (camera == null)
            camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
            camera.transform.RotateAround(transform.position, Vector3.up, turnSpeed * -Input.GetAxis("Mouse X") * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up) * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position -= Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up) * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.ProjectOnPlane(camera.transform.right, Vector3.up) * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            transform.position -= Vector3.ProjectOnPlane(camera.transform.right, Vector3.up) * moveSpeed * Time.deltaTime;
    }
}
