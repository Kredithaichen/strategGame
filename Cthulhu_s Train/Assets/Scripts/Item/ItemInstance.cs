using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInstance : ScriptableObject
{
    [SerializeField]
    private Item referencedItem;

    public Item ReferencedItem
    {
        get { return referencedItem; }
        set { referencedItem = value; }
    }
}
