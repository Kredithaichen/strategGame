using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {

    [SerializeField]
    private GameObject wreckedObject;

    public float radius = 5.0F;
    public float power = 10.0F;

    void breakStuff ()
    {
        if (Input.GetKeyDown(KeyCode.A))//deadBeetlePrefab != null)
        {
            Instantiate(wreckedObject, transform.position, transform.rotation);
            
        }
        Destroy(gameObject);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
