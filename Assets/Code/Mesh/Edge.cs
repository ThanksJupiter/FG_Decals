using UnityEngine;

[System.Serializable]
public struct Edge
{
    public Vertex first;
    public Vertex second;

    public Edge(Vertex _first, Vertex _second)
    {
        first = _first;
        second = _second;
    }

//     public Edge(Vector3 _firstV, Vector3 _secondV, int _firstI, int _secondI)
//     {
//         first = new Vertex(_firstV, _firstI);
//         second = new Vertex(_secondV, _secondI);
//     }
}
