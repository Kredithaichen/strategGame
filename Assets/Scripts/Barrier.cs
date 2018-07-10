using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedModel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            DestroyIt();
    }

    public void DestroyIt()
    {
        var t = transform.GetChild(0).transform;

        Destroy(t.gameObject);
        var obj = Instantiate(destroyedModel, transform.position, transform.rotation, transform);
        obj.GetComponent<Rigidbody>().AddExplosionForce(10, obj.transform.position, 3);

        StartCoroutine(RemovePhysicsFromBricks());
    }

    private IEnumerator RemovePhysicsFromBricks()
    {
        yield return new WaitForSeconds(5.0f);

        var child = transform.GetChild(0);

        for (int i = 0; i < child.childCount; i++)
            Destroy(child.GetChild(i).GetComponent<Rigidbody>());

        Debug.Log("finished");
    }
}
