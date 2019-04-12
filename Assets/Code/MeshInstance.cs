using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInstance : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    public Vector3[] translations = new Vector3[4];
    public Vector3[] eulers = new Vector3[4];
    public Vector3[] scales = new Vector3[]
    {
        Vector3.one,
        Vector3.one,
        Vector3.one,
        Vector3.one
    };

    private Matrix4x4[] matrices = new Matrix4x4[4];

    private void Update()
    {
        for(int i = 0; i < translations.Length; ++i)
        {
            Quaternion rotation = Quaternion.Euler(eulers[i]);
            matrices[i].SetTRS(translations[i], rotation, scales[i]);
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }
}
