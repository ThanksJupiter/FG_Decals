using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SnowTracker : MonoBehaviour
{
    private Mesh pyramidMesh;
    private Mesh snowWallMesh;

    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 travelDirection;

    public SnowTracks tracks;

    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public PyramidShape pyramidShape;
    public SnowWallShape snowWallShape;

    public float tracksInterval = 1f;

    private Vector3 previousSegmentPosition;
    private int currentTriangleOffset = 0;
    private float currentRotation = 0;

    void Start()
    {
        snowWallMesh = new Mesh();
        snowWallMesh.name = "SnowWallMesh";

        //pyramidShape = new PyramidShape(new Vector3(0, 0, 0));
        //snowWallShape = new SnowWallShape(Vector3.zero, 0, 0);

        snowWallMesh.Clear();

        travelDirection = Vector3.forward;
        position = transform.position;

        tracks = new SnowTracks(transform.position);
        tracks.AddSegment(position, currentTriangleOffset, 0);
        previousSegmentPosition = transform.position;

        snowWallMesh.vertices = tracks.GetVertices();
        int[] tmpTris = tracks.GetTriangles();
        snowWallMesh.triangles = tmpTris;

        GetComponent<MeshFilter>().mesh = snowWallMesh;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float rotationToAdd = horizontal * rotationSpeed;
        currentRotation += rotationToAdd;
        travelDirection = Quaternion.Euler(0f, rotationToAdd, 0f) * travelDirection;
        float moveAmount = moveSpeed * vertical * Time.deltaTime;
        position += travelDirection * moveAmount;
        Debug.DrawLine(position, position + travelDirection * 5f, Color.green);

        if (Vector3.Distance(position, previousSegmentPosition) > tracksInterval)
        {
            PlaceNewTrackSegment();
        }

//         snowWallMesh.Clear();
//         snowWallMesh.vertices = snowWallShape.vertices;
//         snowWallMesh.triangles = snowWallShape.GetTriangles();
//         snowWallMesh.RecalculateNormals();

//         for (int i = 0; i < tracks.GetVertices().Length; ++i)
//         {
//             if (i + 1 < tracks.GetVertices().Length)
//             {
//                 Debug.DrawLine(tracks.GetVertices()[i], tracks.GetVertices()[i + 1]);
//             }
//             else
//             {
//                 Debug.DrawLine(tracks.GetVertices()[i], tracks.GetVertices()[0]);
//             }
//         }
    }

    private void PlaceNewTrackSegment()
    {
        currentTriangleOffset += 11;
        tracks.AddSegment(position, currentTriangleOffset, currentRotation);
        previousSegmentPosition = position;

        snowWallMesh.Clear();
        snowWallMesh.vertices = tracks.GetVertices();
        snowWallMesh.triangles = tracks.GetTriangles();
        snowWallMesh.RecalculateNormals();
    }

    [System.Serializable]
    public struct PyramidShape
    {
        public Vector3[] vertices;
        public int[] triangles;

        public Vector3 position;

        public PyramidShape(Vector3 _position)
        {
            vertices = new Vector3[]
            {
                _position + new Vector3(0, 0, 0),
                _position + new Vector3(-1, 0, 0),
                _position + new Vector3(-1, 0, 1),
                _position + new Vector3(0, 0, 1),
                _position + new Vector3(-.5f, 1, .5f)
            };

            triangles = new int[]
            {
                0, 1, 4,
                1, 2, 4,
                2, 3, 4,
                3, 0, 4
            };

            position = _position;
        }

        public void UpdatePosition(Vector3 _delta)
        {
            position += _delta;

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] += _delta;
            }
        }
    }

    [System.Serializable]
    public struct SnowWallShape
    {
        public Vector3[] vertices;
        public Triangle[] triangles;

        public int[] GetTriangles()
        {
            int[] tmpTris = new int[triangles.Length * 3];
            int index = 0;
            for (int i = 0; i < triangles.Length; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    tmpTris[index] = triangles[i].vertices[j];
                    index++;
                }
            }

            return tmpTris;
        }

        public Vector3[] positions;
        public Vector3[] segmentRotations;

        public SnowWallShape(Vector3 _position, int _triangleOffset, float _rotation)
        {
            vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, .5f, 0),
                new Vector3(3, .5f, 0),
                new Vector3(4, 1, 0),
                new Vector3(5, 1, 0),
                new Vector3(5, 0, 0),
                new Vector3(4, 0, 0),
                new Vector3(2.5f, 0, 0),
                new Vector3(1, 0, 0),
            };

            for(int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].x -= 2.5f; // for centering vertices
                vertices[i] = Quaternion.Euler(0f, _rotation, 0f) * vertices[i];
                vertices[i] += _position;
            }

            triangles = new Triangle[]
            {
                new Triangle(0, 1, 2),
                new Triangle(10, 0, 2),
                new Triangle(2, 3, 10),
                new Triangle(10, 3, 9),
                new Triangle(3, 4, 8),
                new Triangle(9, 3, 4),
                new Triangle(9, 4, 8),
                new Triangle(8, 4, 5),
                new Triangle(5, 7, 8),
                new Triangle(5, 6, 7)
            };

            foreach(Triangle t in triangles)
            {
                t.OffsetTriangles(_triangleOffset);
            }

            positions = new Vector3[]
            {
                _position
            };

            segmentRotations = new Vector3[]
            {
                _position
            };
        }
    }

    [System.Serializable]
    public struct SnowTracks
    {
        public int segments;
        public SnowWallShape[] shapes;

        public Vector3[] GetVertices()
        {
            Vector3[] tmpVerts = new Vector3[shapes.Length * 11];

            int index = 0;
            for(int i = 0; i < shapes.Length; ++i)
            {
                for (int j = 0; j < 11; ++j)
                {
                    tmpVerts[index] = shapes[i].vertices[j];
                    index++;
                }
            }

            return tmpVerts;
        }

        public int[] GetTriangles()
        {
            int[] tmpTris = new int[shapes.Length * shapes[0].GetTriangles().Length];

            int index = 0;
            for(int i = 0; i < shapes.Length; ++i)
            {
                int[] tempestTris = shapes[i].GetTriangles();

                for(int j = 0; j < tempestTris.Length; ++j)
                {
                    tmpTris[index] = tempestTris[j];
                    index++;
                }
            }

            return tmpTris;
        }

        public SnowTracks(Vector3 _position)
        {
            segments = 0;
            shapes = new SnowWallShape[segments + 1];
        }

        public void AddSegment(Vector3 _position, int _triangleOffset, float _rotation)
        {
            SnowWallShape[] tmpShapes = new SnowWallShape[segments + 1];
            SnowWallShape newShape = new SnowWallShape(_position, _triangleOffset, _rotation);

            for(int i = 0; i < shapes.Length; ++i)
            {
                tmpShapes[i] = shapes[i];
            }

            tmpShapes[segments] = newShape;
            shapes = tmpShapes;
            segments++;
        }
    }

    // For ease of editor, should maybe try to split up in editor script instead, makes code confusing
    [System.Serializable]
    public struct Triangle
    {
        public int[] vertices;

        public Triangle(int vertex0, int vertex1, int vertex2)
        {
            vertices = new int[3];
            vertices[0] = vertex0;
            vertices[1] = vertex1;
            vertices[2] = vertex2;
        }

        public void OffsetTriangles(int _triangleOffset)
        {
            vertices[0] += _triangleOffset;
            vertices[1] += _triangleOffset;
            vertices[2] += _triangleOffset;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(position, .5f);
    }
}
