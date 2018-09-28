using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable
{
    [SerializeField]
    private List<ItemInstance> items;

    public List<ItemInstance> Items
    {
        get { return items; }
        set { items = value; }
    }
}
