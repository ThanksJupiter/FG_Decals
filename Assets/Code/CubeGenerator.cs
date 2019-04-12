using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeGenerator : MonoBehaviour
{
    public float scale = 1f;
    public Vector3 right;
    public Vector3 up;
    public Vector3 forward;

    public float rotateSpeed = 50f;
    public float scaleIncrease = 2f;

    int directionToModify = 0;

    [SerializeField] private Vector3[] vertices;
    [SerializeField] private int[] triangles;

    private Mesh mesh;

    private MeshFilter filter;

    private void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "FantasticalMesh";

        right = Vector3.right;
        up = Vector3.up;
        forward = Vector3.forward;

        filter.mesh = mesh;

        //vertices = GetCubeVertices(transform.position, Vector3.forward, Vector3.up, scale);
        vertices = GetCubeVertices(transform.position, forward, up, scale);
        triangles = GetCubeTriangles();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        ApplyMeshData();
        mesh.triangles = triangles;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            directionToModify = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            directionToModify = 1;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            scale -= scaleIncrease * Time.deltaTime;
            vertices = GetCubeVertices(transform.position, forward, up, scale);
            ApplyMeshData();
        } else if (Input.GetKey(KeyCode.E))
        {
            scale += scaleIncrease * Time.deltaTime;
            vertices = GetCubeVertices(transform.position, forward, up, scale);
            ApplyMeshData();
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float xRotation = horizontal * rotateSpeed * Time.deltaTime;
        float yRotation = vertical * rotateSpeed * Time.deltaTime;

        if (horizontal != 0 || vertical != 0)
        {
            right = Quaternion.AngleAxis(xRotation, up) * right;
            up = Quaternion.AngleAxis(yRotation, right) * up;
            forward = Quaternion.AngleAxis(xRotation, up) * forward;
            forward = Quaternion.AngleAxis(yRotation, right) * forward;

            ApplyMeshData();
        }

        Debug.DrawLine(transform.position, transform.position + right, Color.red);
        Debug.DrawLine(transform.position, transform.position + up, Color.green);
        Debug.DrawLine(transform.position, transform.position + forward, Color.blue);

        vertices = GetCubeVertices(transform.position, forward, up, scale);
    }

    private Vector3[] GetCubeVertices(Vector3 _position, Vector3 _forward, Vector3 _up, float _scale)
    {
        Vector3[] tmpVerts = new Vector3[8];

        Vector3 right = Vector3.Cross(_up, _forward);

        float halfScale = _scale / 2;
        Vector3 rightOffsetPos = _position + right * halfScale;
        Vector3 leftOffsetPos = _position + -right * halfScale;

        Vector3 centerToVertex = _position + (-_forward + -_up) * halfScale;

        for(int i = 0; i < 4; ++i)
        {
            tmpVerts[i] = centerToVertex + rightOffsetPos;
            tmpVerts[i + 4] = centerToVertex + leftOffsetPos;

            // change angle for potential wheel shape
            centerToVertex = Quaternion.AngleAxis(90f, right) * centerToVertex;
        }

        return tmpVerts;
    }

    private void ApplyMeshData()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    /*private Vector3[] GetCubeVertices(Vector3 _position, Vector3 _forward, Vector3 _up, float _scale)
    {
        Vector3[] tmpVerts = new Vector3[8];

        Vector3 right = Vector3.Cross(_up, _forward);

        float hS = _scale / 2;
        Vector3 rightOffsetPos = _position + right * hS;

        tmpVerts[0] = rightOffsetPos + (-_forward * hS) + (-_up * hS);
        tmpVerts[1] = rightOffsetPos + (-_forward * hS) + (_up * hS);
        tmpVerts[2] = rightOffsetPos + (_forward * hS) + (_up * hS);
        tmpVerts[3] = rightOffsetPos + (_forward * hS) + (-_up * hS);

        Vector3 leftOffsetPos = _position + -right * hS;

        tmpVerts[4] = leftOffsetPos + (-_forward * hS) + (-_up * hS);
        tmpVerts[5] = leftOffsetPos + (-_forward * hS) + (_up * hS);
        tmpVerts[6] = leftOffsetPos + (_forward * hS) + (_up * hS);
        tmpVerts[7] = leftOffsetPos + (_forward * hS) + (-_up * hS);

        return tmpVerts;
    }*/

    private int[] GetCubeTriangles()
    {
        int[] tmpTris = new int[]
        {
            0, 1, 2,
            0, 2, 3,

            4, 5, 0,
            5, 1, 0,

            4, 6, 5,
            7, 6, 4,

            5, 6, 1,
            6, 2, 1,

            3, 6, 7,
            3, 2, 6,

            0, 7, 4,
            0, 3, 7
        };

        return tmpTris;
    }
}
