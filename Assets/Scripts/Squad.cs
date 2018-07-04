using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Squad : MonoBehaviour
{
    private Camera cam;
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject barrierPrefab;

    [SerializeField]
    private int barrierCost;

    [SerializeField]
    private GameObject barrierPlaceholder;

    private CheckCollision collisionCheck;

    [SerializeField]
    private GameObject resourceHandler;

    private List<Resource> resources;

    private bool buildMode;
    private int availableResources;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        agent = GetComponent<NavMeshAgent>();

        barrierPlaceholder.SetActive(false);

        UpdateResources();

        collisionCheck = barrierPlaceholder.GetComponent<CheckCollision>();
    }

    void Update()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit))
                agent.SetDestination(hit.point);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (buildMode)
                barrierPlaceholder.SetActive(false);
            else
                barrierPlaceholder.SetActive(true);

            buildMode = !buildMode;
            ToggleResourceSphere();
        }
            
        if (buildMode)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                var rs = 0;

                var count = resources.Count;

                for (int i = 0; i < count; i++)
                {
                    if (resources[i] == null)
                    {
                        resources.RemoveAt(i);
                        count--;
                    }
                }

                foreach (var resource in resources)
                {
                    var dist = (resource.transform.position - hit.point).magnitude;

                    if (dist <= resource.AvailabilityRadius)
                        rs += resource.Amount;
                }

                availableResources = rs;

                barrierPlaceholder.transform.position = hit.point + Vector3.up;
                barrierPlaceholder.transform.RotateAround(hit.point, Vector3.up, 30.0f * Input.GetAxis("Mouse ScrollWheel"));

                if (Input.GetMouseButtonDown(0) && rs >= barrierCost && !collisionCheck.CurrentlyColliding)
                {
                    var sortedList = resources.OrderBy(r => (r.transform.position - hit.point).magnitude);
                    var remainingCost = barrierCost;

                    foreach (var r in sortedList)
                    {
                        if (r.Amount >= remainingCost)
                        {
                            r.Amount -= remainingCost;
                            remainingCost = 0;
                            break;
                        }

                        remainingCost -= r.Amount;
                        r.Amount = 0;
                    }

                    if (remainingCost <= 0)
                        Instantiate(barrierPrefab, barrierPlaceholder.transform.position, barrierPlaceholder.transform.rotation);

                    UpdateResources();
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.color = Color.blue;

        if (buildMode)
        {
            GUI.Label(new Rect(20, 20, 200, 20), "Build Mode");
            GUI.Label(new Rect(20, 40, 100, 20), "Resources: " + availableResources);
        }
    }

    private void UpdateResources()
    {
        resources = resourceHandler.GetComponentsInChildren<Resource>().ToList();
    }

    private void ToggleResourceSphere()
    {
        foreach (var resource in resources)
            resource.ToggleSphere();
    }
}
