using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    private bool currentlyColliding;

    public bool CurrentlyColliding
    {
        get { return currentlyColliding; }
        set { currentlyColliding = value; }
    }

    void OnTriggerStay(Collider other)
    {
        currentlyColliding = true;
        GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.4f);
    }

    void OnTriggerExit(Collider other)
    {
        currentlyColliding = false;
        GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 0.4f);
    }
}
