using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

[RequireComponent(typeof(Inventory))]
public class Character : Interactable
{
    [SerializeField]
    private DialogHandler dialogHandler;

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    public override void Interact()
    {
        base.Interact();
        dialogHandler.StartDialog(ObjectName);
    }
}
