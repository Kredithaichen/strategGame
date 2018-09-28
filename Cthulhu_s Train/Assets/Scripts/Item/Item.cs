using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : ScriptableObject
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private Sprite image;

    public string ItemName
    {
        get { return itemName; }
    }

    public Sprite Image
    {
        get { return image; }
        set { image = value; }
    }
}
