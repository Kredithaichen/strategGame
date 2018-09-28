using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private float interactionRadius = 3.0f;

    [SerializeField]
    private string objectName;

    public string ObjectName
    {
        get { return objectName; }
        set { objectName = value; }
    }

    // Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    public virtual void Interact()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Enter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Exit()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
