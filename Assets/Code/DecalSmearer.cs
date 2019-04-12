using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DecalSmearer : MonoBehaviour
{
    public Mesh mesh;
    //public Decal decal;

    //public Vector3[] vertices;

    public MeshShape meshShape;
    public FaceData[] facePositions;
    public float scale = 1;
    public Face[] faces;

    public Vector3[] cubeVertices;
    public int[] cubeTriangles;

    //public Edge[] edges = new Edge[4];

    //public float moveSpeed = 5f;
    //public float rotationSpeed = 5f;
    //
    //public float segmentInterval = 1f;
    //
    //private Vector3 previousSegmentPosition;
    //private int currentTriangleOffset = 0;
    //private float currentRotation = 0;

    public Vector3 normal;

    // Start is called before the first frame update
    void Start()
    {
        meshShape.faceData = facePositions;

        mesh = new Mesh();
        mesh.name = "CubeMesh";
        GetComponent<MeshFilter>().mesh = mesh;

        cubeVertices = new Vector3[facePositions.Length * 4];
        cubeTriangles = new int[facePositions.Length * 6];
        int index = 0;

        faces = new Face[facePositions.Length];
        for (int i = 0; i < facePositions.Length; ++i)
        {
            faces[i] = new Face(facePositions[i].position, facePositions[i].normal, scale);
            for (int j = 0; j < faces[i].vertices.Length; ++j)
            {
                cubeVertices[index] = faces[i].vertices[j].position;
                index++;
            }
        }

        int triIndex = 0;
        int iterations = 0;
        for (int i = 0; i < faces.Length; ++i)
        {
            int[] tmpTris = faces[i].GetTriangles();
            for (int j = 0; j < tmpTris.Length; ++j)
            {
                cubeTriangles[triIndex] = tmpTris[j] + iterations;
                triIndex++;
            }

            iterations += 4;
        }

        mesh.vertices = cubeVertices;
        mesh.triangles = cubeTriangles;
    }

    private void Update()
    {
/*
        Vector3 right;
        Vector3 forward;
        Vector3 up;

        if (normal != Vector3.up)
        {
            right = Vector3.Cross(Vector3.up, normal);
            forward = normal;
            up = Vector3.Cross(forward, right);
        }
        else
        {
            up = normal;
            forward = Vector3.Cross(Vector3.right, up);
            right = Vector3.Cross(up, forward);
        }

        Debug.DrawLine(transform.position, transform.position + right * 1f, Color.red);
        Debug.DrawLine(transform.position, transform.position + up * 1f, Color.green);
        Debug.DrawLine(transform.position, transform.position + forward * 1f, Color.blue);*/
    }

    [System.Serializable]
    public struct Decal
    {
        public Vector3[] vertices;
        public int[] triangles;

        public Decal(Vector3 _position)
        {
            vertices = new Vector3[]
            {
                new Vector3(-.5f, 0, 0),
                new Vector3(.5f, 0, 0),
                new Vector3(-.5f, 0, 1),
                new Vector3(.5f, 0, 1),
            };

            triangles = new int[]
            {
                0, 2, 3,
                0, 3, 1
            };
        }
    }
}
