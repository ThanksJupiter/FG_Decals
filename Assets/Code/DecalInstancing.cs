using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalInstancing : MonoBehaviour
{
    const int MAX_DECALS = 1023;

    public Mesh mesh;
    public Material material;
    public Vector3 scale = Vector3.one;

    public Matrix4x4[] matrices = new Matrix4x4[MAX_DECALS];
    private int currentDecalAmount = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                PlaceDecal(hit.point + hit.normal * .01f, hit.normal);
            }
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
    }

    private void PlaceDecal(Vector3 _position, Vector3 _normal)
    {
        Quaternion rotation = Quaternion.LookRotation(-_normal, Vector3.up);
        matrices[currentDecalAmount].SetTRS(_position, rotation, scale);
        currentDecalAmount++;
        if (currentDecalAmount == MAX_DECALS)
        {
            currentDecalAmount = 0;
        }
    }
}
