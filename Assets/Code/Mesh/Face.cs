using UnityEngine;

[System.Serializable]
public struct Face
{
    public Vertex[] vertices;
    public Edge[] edges;

    public Triangle first;
    public Triangle second;

    /// <summary>
    /// Please only input 4 edges or god knows what will happen
    /// </summary>
    /// <param name="_edges"></param>
    public Face(Edge[] _edges)
    {
        edges = _edges;

        vertices = new Vertex[]
        {
            new Vertex(edges[0].first),
            new Vertex(edges[0].second),
            new Vertex(edges[2].first),
            new Vertex(edges[2].second),
        };

        first = new Triangle(vertices[0], vertices[2], vertices[3]);
        second = new Triangle(vertices[0], vertices[3], vertices[1]);
    }

    /// <summary>
    /// Please only input 4 vertices or god knows what will happen
    /// </summary>
    /// <param name="_edges"></param>
    public Face(Vector3[] _vertices)
    {
        vertices = new Vertex[]
        {
            new Vertex(_vertices[0], 0),
            new Vertex(_vertices[1], 1),
            new Vertex(_vertices[2], 2),
            new Vertex(_vertices[3], 3),
        };

        edges = new Edge[]
        {
            new Edge(vertices[0], vertices[1]),
            new Edge(vertices[1], vertices[2]),
            new Edge(vertices[2], vertices[3]),
            new Edge(vertices[3], vertices[0]),
        };

        first = new Triangle(vertices[0], vertices[2], vertices[3]);
        second = new Triangle(vertices[0], vertices[1], vertices[2]);
    }

    public Face(Vector3 _position, Vector3 _normal, float _scale)
    {
        Vector3 forward;
        Vector3 right;
        Vector3 up;

        if (_normal != Vector3.up)
        {
            forward = -_normal;
            right = Vector3.Cross(Vector3.up, forward);
            up = Vector3.Cross(forward, right);
        }
        else
        {
            up = Vector3.Cross(Vector3.right, _normal);
            forward = -_normal;
            right = Vector3.Cross(up, forward);
        }

        float halfScale = _scale / 2;

        // todo allow "up" input for deciding rotation
        Vector3[] positions = new Vector3[]
        {
            (_position * _scale ) + (-right * halfScale) + (-up * halfScale),
            (_position * _scale ) + (-right * halfScale) + (up * halfScale),
            (_position * _scale ) + (right * halfScale) + (up * halfScale),
            (_position * _scale ) + (right * halfScale) + (-up * halfScale)
        };

        vertices = new Vertex[]
        {
            new Vertex(positions[0], 0),
            new Vertex(positions[1], 1),
            new Vertex(positions[2], 2),
            new Vertex(positions[3], 3),
        };

        edges = new Edge[]
        {
            new Edge(vertices[0], vertices[1]),
            new Edge(vertices[1], vertices[2]),
            new Edge(vertices[2], vertices[3]),
            new Edge(vertices[3], vertices[0]),
        };

        first = new Triangle(vertices[0], vertices[2], vertices[3]);
        second = new Triangle(vertices[0], vertices[1], vertices[2]);
    }

    public int[] GetTriangles()
    {
        return new int[]
        {
            first.first.index,
            first.second.index,
            first.third.index,
            second.first.index,
            second.second.index,
            second.third.index
        };
    }

    public Vector3[] GetVertices()
    {
        return new Vector3[]
        {
            vertices[0].position,
            vertices[1].position,
            vertices[2].position,
            vertices[3].position,
        };
    }
}
