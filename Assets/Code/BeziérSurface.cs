using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BeziérSurface : MonoBehaviour
{
    private Mesh mesh;

    [SerializeField] private Vector3[] vertices;
    [SerializeField] private int[] triangles;

    public int[] tmpTris = new int[((5 - 1) * 2) * (4 - 1) * 3];

    public Vector3[][] verticalBeziers = new Vector3[4][];

    public Vector3[] acrossBezier;

    private float segmentDistance = 0f;
    public int segments = 5;

    [Range(0, 1)]
    public float xAlpha;

    [Range(0, 1)]
    public float yAlpha;

    [SerializeField] private Vector3 spherePosition;

    void Start()
    {
        mesh = new Mesh();

        verticalBeziers[0] = new Vector3[]
        {
            new Vector3(-1f, 0, 0),
            new Vector3(1f, 0, 0),
            new Vector3(-1f, 1, 0),
            new Vector3(1f, 1, 0)
        };

        verticalBeziers[1] = new Vector3[]
        {
            new Vector3(-1f, 0, 1),
            new Vector3(1f, 0, 1),
            new Vector3(-1f, 1, 1),
            new Vector3(1f, 1, 1)
        };

        verticalBeziers[2] = new Vector3[]
        {
            new Vector3(-1f, 0, 2),
            new Vector3(1f, 0, 2),
            new Vector3(-1f, 1, 2),
            new Vector3(1f, 1, 2)
        };

        verticalBeziers[3] = new Vector3[]
        {
            new Vector3(-1f, 0, 3),
            new Vector3(1f, 0, 3),
            new Vector3(-1f, 1, 3),
            new Vector3(1f, 1, 3)
        };

        segmentDistance = 1f / (segments - 1);

        acrossBezier = GetHorizontalPoints(yAlpha);

        vertices = CalculateVertices(segments);
        tmpTris = CalculateTriangles(segments);

        mesh.vertices = vertices;
        mesh.triangles = tmpTris;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        acrossBezier = GetHorizontalPoints(yAlpha);
        spherePosition = GetPointOnCurve(acrossBezier, xAlpha);

        segmentDistance = 1f / (segments - 1);

        vertices = CalculateVertices(segments);
        tmpTris = CalculateTriangles(segments);

        mesh.vertices = vertices;
        mesh.triangles = tmpTris;
        mesh.RecalculateNormals();
    }

    public Vector3[] GetHorizontalPoints(float t)
    {
        Vector3[] tmpPts = new Vector3[4];

        tmpPts[0] = GetPointOnCurve(verticalBeziers[0], t);
        tmpPts[1] = GetPointOnCurve(verticalBeziers[3], t);
        tmpPts[2] = GetPointOnCurve(verticalBeziers[1], t);
        tmpPts[3] = GetPointOnCurve(verticalBeziers[2], t);

        return tmpPts;
    }

    public Vector3 GetPointOnCurve(Vector3[] bezier, float t)
    {
        Vector3 a = Vector3.Lerp(bezier[0], bezier[2], t);
        Vector3 b = Vector3.Lerp(bezier[2], bezier[3], t);
        Vector3 c = Vector3.Lerp(bezier[3], bezier[1], t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(spherePosition, .5f);

        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; ++i)
        {
            Gizmos.DrawWireSphere(vertices[i], .2f);
        }
    }

    private Vector3[] CalculateVertices(int segments)
    {
        Vector3[] tmpVerts = new Vector3[segments * 4];

        int index = 0;
        float iAlpha = 0;
        for(int i = 0; i < segments; ++i)
        {
            Vector3[] tmpBez = GetHorizontalPoints(iAlpha);
            float jAlpha = 0;
            for(int j = 0; j < 4; j++)
            {
                tmpVerts[index] = GetPointOnCurve(tmpBez, jAlpha);
                jAlpha += .33f;
                index++;
            }

            iAlpha += segmentDistance;
        }

        return tmpVerts;
    }

    private int[] CalculateTriangles(int segments)
    {
        int length = ((segments) * 2) * (4 - 1) * 3;
        int[] tmpTris = new int[length];
        Debug.Log(length);

        int index = 0;
        for (int i = 0; i < tmpTris.Length / 6; i++)
        {
            tmpTris[index + 0] = i;
            tmpTris[index + 1] = i + 1;
            tmpTris[index + 2] = i + 4 + 1;

            tmpTris[index + 3] = i;
            tmpTris[index + 4] = i + 4 + 1;
            tmpTris[index + 5] = i + 4;

            index += 6;
        }

        /*for (int i = 0; i < tmpTris.Length / 6; i++)
        {
            tmpTris[i + 0] = i;
            tmpTris[i + 1] = i + segments;
            tmpTris[i + 2] = i + segments + 1;

            tmpTris[i + 3] = i;
            tmpTris[i + 4] = i + segments + 1;
            tmpTris[i + 5] = i + 1;
        }*/

        return tmpTris;
    }
}
