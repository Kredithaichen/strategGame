using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenuHandler : MonoBehaviour
{
    public delegate void ChooseMenuItem();

    public event ChooseMenuItem ChooseItem1, ChooseItem2, ChooseItem3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickOnButton1()
    {
        if (ChooseItem1 != null)
            ChooseItem1();
    }

    public void ClickOnButton2()
    {
        if (ChooseItem2 != null)
            ChooseItem2();
    }

    public void ClickOnButton3()
    {
        if (ChooseItem3 != null)
            ChooseItem3();
    }

    public void ClickOnButton4()
    { }
}
