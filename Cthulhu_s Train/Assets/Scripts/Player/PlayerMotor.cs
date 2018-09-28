using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private float turnSpeed = 5.0f;

    [SerializeField]
    private float moveSpeed = 5.0f;

    private float currentSpeed;
    private Vector3 moveTarget;
    private Vector3 moveDirection;
    private float hitdist;

    private PlayerInputHandler inputHandler;
    private CharacterController characterController;

    // Use this for initialization
    void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }
	
	// Update is called once per frame
	void Update()
	{
	    // turn
	    Ray ray;

	    if (inputHandler.GamepadActive)
	    {
	        var angle = Vector3.SignedAngle(transform.forward,
	                        new Vector3(-Input.GetAxis("Joystick Right X"), 0, Input.GetAxis("Joystick Right Y")),
	                        Vector3.up) / 180 * Mathf.PI;

	        transform.Rotate(Vector3.up, 2.0f * turnSpeed * angle);

	        /*if (currentSpeed > 0.5f)
            {

                moveDirection = new Vector3(-Input.GetAxis("Joystick Right X"), 0.0f, Input.GetAxis("Joystick Right Y"));
                transform.LookAt(transform.position - 3 * moveDirection);
            }
            else
                moveDirection = transform.forward;*/
	    }
	    else if (Math.Abs(Input.GetAxis("Mouse X")) > 0.001f || Math.Abs(Input.GetAxis("Mouse Y")) > 0.001f)
	    {
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

	        var playerPlane = new Plane(Vector3.up, transform.position);

	        if (playerPlane.Raycast(ray, out hitdist))
	        {
	            var targetPoint = ray.GetPoint(hitdist);
	            var targetRotation = Quaternion.LookRotation(transform.position - targetPoint);
	            transform.rotation =
	                Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	        }
	    }

	    // move
	    characterController.Move(moveSpeed * currentSpeed * Time.deltaTime * Vector3.forward * Input.GetAxis("Vertical"));
	    characterController.Move(moveSpeed * currentSpeed * Time.deltaTime * Vector3.right * Input.GetAxis("Horizontal"));

	    // calculate speed
	    currentSpeed =
	        Mathf.Sqrt(Mathf.Pow(Input.GetAxis("Vertical"), 2) + Mathf.Pow(Input.GetAxis("Horizontal"), 2));
    }
}
