using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Meshmerize : MonoBehaviour
{
    const int MAX_DECALS = 1000;
    const int MAX_VERTICES = MAX_DECALS * 4;
    const int MAX_TRIANGLE_INDICES = MAX_DECALS * 6;
    Vector3[] myVertices = new Vector3[MAX_VERTICES];
    int[] myTriangles = new int[MAX_TRIANGLE_INDICES];

    Mesh myMesh;

    [SerializeField] int currentVertexIndex = 0;
    [SerializeField] int currentTriangleIndex = 0;

    private void Start()
    {
        myMesh = new Mesh();
        myMesh.name = "MyVeryOwnMesh";

        GetComponent<MeshFilter>().mesh = myMesh;
        ResetTriangles();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                PlaceQuad(hit.point + hit.normal * .01f, hit.normal, hit.transform.up);
                ResetTriangles();
            }
        }
    }

    private void PlaceQuad(Vector3 _position, Vector3 _normal, Vector3 _localUp)
    {
        Quaternion rotToNormal = Quaternion.FromToRotation(Vector3.forward, -_normal);

        Vector3 right = rotToNormal * Vector3.right;
        Vector3 up = rotToNormal * Vector3.up;

        Quaternion upRotToNormal = Quaternion.FromToRotation(up, _localUp);

        up = upRotToNormal * up;
        right = upRotToNormal * right;

        // placement on floor or ceiling hack, should correspond to player look direction or similar
        if (_normal == Vector3.up || _normal == Vector3.down)
        {
            up = Vector3.forward;
        }

        for (int i = 0; i < 4; i++)
        {
            myVertices[i + currentVertexIndex] = _position;
        }

        myVertices[0 + currentVertexIndex] += (-up * .5f) + (-right * .5f);
        myVertices[1 + currentVertexIndex] += (up * .5f) + (-right * .5f);
        myVertices[2 + currentVertexIndex] += (up * .5f) + (right * .5f);
        myVertices[3 + currentVertexIndex] += (-up * .5f) + (right * .5f);

        if (_normal != Vector3.down)
        {
            myTriangles[0 + currentTriangleIndex] = 0 + currentVertexIndex;
            myTriangles[1 + currentTriangleIndex] = 1 + currentVertexIndex;
            myTriangles[2 + currentTriangleIndex] = 3 + currentVertexIndex;

            myTriangles[3 + currentTriangleIndex] = 1 + currentVertexIndex;
            myTriangles[4 + currentTriangleIndex] = 2 + currentVertexIndex;
            myTriangles[5 + currentTriangleIndex] = 3 + currentVertexIndex;
        }
        else // placement on ceiling hack, rotate normals to face down
        {
            myTriangles[0 + currentTriangleIndex] = 3 + currentVertexIndex;
            myTriangles[1 + currentTriangleIndex] = 1 + currentVertexIndex;
            myTriangles[2 + currentTriangleIndex] = 0 + currentVertexIndex;

            myTriangles[3 + currentTriangleIndex] = 3 + currentVertexIndex;
            myTriangles[4 + currentTriangleIndex] = 2 + currentVertexIndex;
            myTriangles[5 + currentTriangleIndex] = 1 + currentVertexIndex;
        }

        currentVertexIndex += 4;
        currentTriangleIndex += 6;

        // start replacing decals if maximum amount reached
        if (currentVertexIndex == MAX_VERTICES)
            currentVertexIndex = 0;

        if (currentTriangleIndex == MAX_TRIANGLE_INDICES)
            currentTriangleIndex = 0;
    }

    private void ResetTriangles()
    {
        myMesh.Clear();

        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangles;

        myMesh.RecalculateNormals();
    }
}
