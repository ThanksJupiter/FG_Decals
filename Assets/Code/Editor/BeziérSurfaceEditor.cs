using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BeziérSurface))]
public class BeziérSurfaceEditor : Editor
{
    const string undoMsg = "Undo move beziér vector";

    public void OnSceneGUI()
    {
        BeziérSurface bs = target as BeziérSurface;

        if (bs.verticalBeziers[0] == null)
        {
            return;
        }

        Vector3[][] vectors = new Vector3[][]
        {
            bs.verticalBeziers[0],
            bs.verticalBeziers[1],
            bs.verticalBeziers[2],
            bs.verticalBeziers[3]
        };
        //Vector3[] vectors0 = new Vector3[bs.verticalBeziers[0].Length];
        //Vector3[] vectors1 = new Vector3[bs.verticalBeziers[1].Length];
        //Vector3[] vectors2 = new Vector3[bs.verticalBeziers[2].Length];
        //Vector3[] vectors3 = new Vector3[bs.verticalBeziers[3].Length];

        EditorGUI.BeginChangeCheck();

        for(int i = 0; i < bs.verticalBeziers.Length; ++i)
        {
            for (int j = 0; j < vectors[i].Length; ++j)
            {
                vectors[i][j] = Handles.PositionHandle(bs.verticalBeziers[i][j], Quaternion.identity);
            };

            Handles.DrawBezier(
                bs.verticalBeziers[i][0],
                bs.verticalBeziers[i][1],
                bs.verticalBeziers[i][2],
                bs.verticalBeziers[i][3],
                Color.red,
                null,
                2f
            );
        }

        Handles.DrawBezier(
            bs.acrossBezier[0],
            bs.acrossBezier[1],
            bs.acrossBezier[2],
            bs.acrossBezier[3],
            Color.blue,
            null,
            2f
        );

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(bs, undoMsg);
            bs.verticalBeziers[0] = vectors[0];
            bs.verticalBeziers[1] = vectors[1];
            bs.verticalBeziers[2] = vectors[2];
            bs.verticalBeziers[3] = vectors[3];
        }
    }
}
