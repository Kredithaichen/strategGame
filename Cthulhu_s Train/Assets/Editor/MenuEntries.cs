using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuEntries : MonoBehaviour
{
    [MenuItem("Assets/Create/Items/Item")]
    public static void CreateItem()
    {
        ScriptableObjectUtility.CreateAsset<Item>();
    }

    [MenuItem("Assets/Create/Items/ItemInstance")]
    public static void CreateItemInstance()
    {
        ScriptableObjectUtility.CreateAsset<ItemInstance>();
    }
}
