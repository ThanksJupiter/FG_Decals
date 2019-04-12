using UnityEngine;

[System.Serializable]
public struct Triangle
{
    public Vertex first;
    public Vertex second;
    public Vertex third;

    public Triangle(Vertex _first, Vertex _second, Vertex _third)
    {
        first = _first;
        second = _second;
        third = _third;
    }
}
