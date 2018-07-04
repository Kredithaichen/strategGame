using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTarget : MonoBehaviour
{
    [SerializeField]
    private Color gizmosColor = Color.blue;

    [SerializeField]
    private float gizmosRadius = 0.5f;
    
    public Transform SnapTargetHandle
    {
        get { return transform; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;

        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position, gizmosRadius);

        Gizmos.color = oldColor;
    }
}
