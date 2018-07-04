using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapObject : MonoBehaviour
{
    public SnapTarget testTarget;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SnapToTarget(SnapTarget target)
    {
        transform.position = target.SnapTargetHandle.position;
        transform.rotation = target.SnapTargetHandle.rotation;
    }
}
