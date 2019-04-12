using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TrackMesh : MonoBehaviour
{
    [SerializeField] Vector3 travelDirection;
    [SerializeField] Vector3 currentPosition;
    [SerializeField] Vector3 currentRight;

    public float moveSpeed = 5f;
    public float rotateSpeed = 1f;

    public Vector3[] myVertices;
    public int[] myTriangles;
    
    Mesh myMesh;

    private void Start()
    {
        myMesh = new Mesh();
        myMesh.name = "MyVeryOwnMesh";

        travelDirection = transform.forward;

        ResetTriangles();
    }

    private void Update()
    {
        Steer();

        for (int i = 0; i < myVertices.Length; i++)
        {
            if (i + 1 < myVertices.Length)
            {
                Debug.DrawLine(myVertices[i], myVertices[i + 1]);
            }
            else
            {
                Debug.DrawLine(myVertices[i], myVertices[0]);
            }
        }

        ResetTriangles();
    }

    private void Steer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        travelDirection = Quaternion.Euler(0f, horizontal * rotateSpeed * Time.deltaTime, 0f) * travelDirection;

        float directionMultiplier = vertical * moveSpeed * Time.deltaTime;
        currentPosition += travelDirection * directionMultiplier;

        currentRight = Vector3.Cross(Vector3.up, travelDirection);
    }

    private void ResetTriangles()
    {
        myMesh.Clear();

        myMesh.vertices = myVertices;
        myMesh.triangles = myTriangles;

        myMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = myMesh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(currentPosition, 0.5f);
        Gizmos.DrawLine(currentPosition, currentPosition + travelDirection * 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(currentPosition, currentPosition + currentRight * 1f);
    }
}
