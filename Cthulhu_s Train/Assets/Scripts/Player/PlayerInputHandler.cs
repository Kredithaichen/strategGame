using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private bool mouseActive;
    private bool gamepadActive;

    private bool hasChanged;

    public bool MouseActive
    {
        get { return mouseActive; }
        set
        {
            if (mouseActive == value)
                return;

            mouseActive = value;
            hasChanged = true;

            SetKeyHints(value);
            SetControllerHints(!value);
        }
    }

    public bool GamepadActive
    {
        get { return gamepadActive; }
        set
        {
            gamepadActive = value;

            SetKeyHints(!value);
            SetControllerHints(value);
        }
    }

    public bool DoAffirmativeAction
    {
        get { return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0); }
    }

    public bool DoCancelAction
    {
        get { return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1); }
    }

    public bool DoMenuOpen
    {
        get { return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7); }
    }

    void Start()
    {
		
	}

	void Update()
	{
		CheckInputDevice();
	}

    public void CheckInputDevice()
    {
        if (Math.Abs(Input.GetAxis("Joystick Right X")) > 0.01f ||
            Math.Abs(Input.GetAxis("Joystick Right Y")) > 0.01f ||
            Math.Abs(Input.GetAxis("Joystick Left X")) > 0.01f ||
            Math.Abs(Input.GetAxis("Joystick Left Y")) > 0.01f)
        {
            GamepadActive = true;
            MouseActive = false;
            return;
        }

        if (Math.Abs(Input.GetAxis("Mouse X")) > 0.01f ||
            Math.Abs(Input.GetAxis("Mouse Y")) > 0.01f)
        {
            GamepadActive = false;
            MouseActive = true;
        }
    }

    private void SetControllerHints(bool visible)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("ControllerHint"))
        {
            var c = obj.GetComponent<CanvasGroup>();
            if (c != null)
            {
                if (visible)
                {
                    c.alpha = 1.0f;
                    c.blocksRaycasts = true;
                    c.interactable = true;
                }
                else
                {
                    c.alpha = 0.0f;
                    c.blocksRaycasts = false;
                    c.interactable = false;
                }
            }
        }
    }

    private void SetKeyHints(bool visible)
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("KeyHint"))
        {
            var c = obj.GetComponent<CanvasGroup>();
            if (c != null)
            {
                if (visible)
                {
                    c.alpha = 1.0f;
                    c.blocksRaycasts = true;
                    c.interactable = true;
                }
                else
                {
                    c.alpha = 0.0f;
                    c.blocksRaycasts = false;
                    c.interactable = false;
                }
            }
        }
    }

    public void RefreshView()
    {
        MouseActive = mouseActive;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 100), gamepadActive ? "gamepad" : "mouse & keyboard");
    }
}
