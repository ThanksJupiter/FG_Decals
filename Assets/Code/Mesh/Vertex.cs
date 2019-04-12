using UnityEngine;

[System.Serializable]
public struct Vertex
{
    public Vector3 position;
    public int index;

    public Vertex(Vector3 _position, int _index)
    {
        position = _position;
        index = _index;
    }

    public Vertex(Vertex _vertex)
    {
        position = _vertex.position;
        index = _vertex.index;
    }
}
