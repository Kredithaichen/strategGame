using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    private string resourceName;

    [SerializeField]
    private int amount;

    [SerializeField]
    private int stability;

    [SerializeField]
    private float availabilityRadius;

    private Transform sphere;

    public int Amount
    {
        get { return amount; }
        set { amount = value; if (value == 0) Destroy(gameObject); }
    }

    public float AvailabilityRadius
    {
        get { return availabilityRadius; }
        set { availabilityRadius = value; }
    }

    // Use this for initialization
    void Start()
    {
        sphere = transform.GetChild(0);
        sphere.localScale = availabilityRadius * Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleSphere()
    {
        sphere.gameObject.SetActive(!sphere.gameObject.activeSelf);
    }
}
