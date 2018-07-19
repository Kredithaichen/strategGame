using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapGrid : MonoBehaviour
{
    [SerializeField]
    private float mapWidth = 10.0f, mapDepth = 10.0f;
    [SerializeField]
    private int mapSizeX = 10, mapSizeZ = 10;
    [SerializeField]
    private float cellSizeX, cellSizeZ;

    void Start()
    {
        CreateGridMesh();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ClickOnGrid(Camera.main);
    }

    private void CreateGridMesh()
    {
        var mesh = new Mesh();

        var verticesCount = (mapSizeX + 1) * (mapSizeZ + 1);
        var vertices = new Vector3[verticesCount];
        var uv = new Vector2[verticesCount];

        cellSizeX = mapWidth / mapSizeX;
        cellSizeZ = mapDepth / mapSizeZ;

        var offsetX = mapWidth / 2;
        var offsetZ = mapDepth / 2;

        for (int i = 0; i < mapSizeX + 1; i++)
        {
            for (int j = 0; j < mapSizeZ + 1; j++)
            {
                var index = i * (mapSizeZ + 1) + j;
                vertices[index] = new Vector3(i * cellSizeX - offsetX, 0, j * cellSizeZ - offsetZ);
                uv[index] = new Vector2((float)i / mapSizeX, (float)j / mapSizeZ);
            }
        }

        var triangles = new int[mapSizeZ * mapSizeX * 2 * 3];

        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeZ; j++)
            {
                var lt = i * (mapSizeZ + 1) + j;
                var rt = i * (mapSizeZ + 1) + j + 1;
                var lb = (i + 1) * (mapSizeZ + 1) + j;
                var rb = (i + 1) * (mapSizeZ + 1) + j + 1;

                var index = (i * mapSizeZ + j) * 6;
                triangles[index + 0] = lt;
                triangles[index + 1] = rt;
                triangles[index + 2] = lb;

                triangles[index + 3] = lb;
                triangles[index + 4] = rt;
                triangles[index + 5] = rb;
            }
        }

        mesh.name = "MapGrid";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(mapSizeX, mapSizeZ);
    }

    private Vector2 ClickOnGrid(Camera cam)
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        var offsetX = mapWidth / 2;
        var offsetZ = mapDepth / 2;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            var x = Mathf.Floor((hit.point.x + offsetX) / cellSizeX);
            var z = Mathf.Floor((hit.point.z + offsetZ) / cellSizeZ);

            //Debug.Log(new Vector2(x, z));

            return new Vector2(x, z);
        }
        else
            return new Vector2(-1, -1);
    }
}
