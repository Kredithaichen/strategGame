using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
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

    private bool mouseActive;
    private bool gamepadActive;

    private CharacterController characterController;

    [SerializeField]
    private MainMenuHandler mainMenuHandler;
    private PlayerInputHandler inputHandler;

    [SerializeField]
    private GameObject dialogPanel;

    [SerializeField]
    private GameObject containerPanel;

    private bool menuActive;
    private bool dialogActive;
    private bool chestOpen;

    private Interactable selectedObject;

    public PlayerInputHandler InputHandler
    {
        get { return inputHandler; }
        set { inputHandler = value; }
    }

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        if (Math.Abs(Input.GetAxis("Joystick Right X")) > 0.001f || Math.Abs(Input.GetAxis("Joystick Right Y")) > 0.001f)
            gamepadActive = true;
        else
            gamepadActive = false;

        if (!dialogActive && !chestOpen && InputHandler.DoAffirmativeAction)
            Interact();
    }

    void FixedUpdate()
    {
        if (menuActive)
        {
            if (inputHandler.DoMenuOpen)
            {
                menuActive = false;
                mainMenuHandler.gameObject.SetActive(false);
            }
        }
        else if (!dialogActive && !chestOpen)
        {
            if (!menuActive && inputHandler.DoMenuOpen)
            {
                menuActive = true;
                mainMenuHandler.gameObject.SetActive(true);
                return;
            }

            // turn
            Ray ray;

            if (gamepadActive)
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
        else
        {
            if (inputHandler.DoCancelAction)
            {
                if (dialogActive)
                    EndDialog();
                else if (chestOpen)
                    CloseChest();
            }
        }
    }

    void LateUpdate()
    {
        playerCamera.transform.position =
            new Vector3(transform.position.x, playerCamera.transform.position.y, transform.position.z);
    }

    void OnGUI()
    {
        /*if (currentSpeed > 0.5f)
            GUI.Label(new Rect(0, 0, 200, 100), "Running at " + currentSpeed.ToString("F2"));
        else
            GUI.Label(new Rect(0, 0, 200, 100), "Walking at " + currentSpeed.ToString("F2"));

        GUI.Label(new Rect(0, 20, 200, 100), "Direction " + moveDirection.ToString());*/

        /*if (selectedObject != null)
        {
            var obj = selectedObject.GetComponent<Interactable>();
            GUI.Label(new Rect(0, 40, 300, 100), obj.ObjectName);
        }*/
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - 3 * moveDirection, 0.3f);
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<Interactable>();
        if (obj != null)
        {
            obj.Enter();

            if (selectedObject != null && !obj.Equals(selectedObject))
                selectedObject.Exit();

            selectedObject = obj;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<Interactable>();
        if (obj != null && obj.Equals(selectedObject))
        {
            obj.Exit();
            selectedObject = null;
        }
    }

    public void StartDialog()
    {
        dialogActive = true;
        dialogPanel.SetActive(true);
    }

    public void EndDialog()
    {
        dialogActive = false;
        dialogPanel.SetActive(false);

        var obj = selectedObject.GetComponent<Character>();
        if (obj != null)
            obj.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OpenChest()
    {
        chestOpen = true;
        containerPanel.SetActive(true);
        containerPanel.GetComponent<ContainerHandler>().Show(selectedObject.GetComponent<Container>());
    }

    public void CloseChest()
    {
        chestOpen = false;
        containerPanel.SetActive(false);
        containerPanel.GetComponent<ContainerHandler>().Close();

        var obj = selectedObject.GetComponent<Container>();
        if (obj != null)
            obj.transform.GetChild(0).gameObject.SetActive(true);
    }

    void Interact()
    {
        if (selectedObject != null)
        {
            if (selectedObject is Character)
                StartDialog();

            if (selectedObject is Container)
                OpenChest();

            selectedObject.Interact();
        }
    }

    public void CloseMenu()
    {
        menuActive = false;
    }
}
