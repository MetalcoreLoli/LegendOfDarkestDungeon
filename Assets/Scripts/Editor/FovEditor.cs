using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FovEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = target as FieldOfView;

        Handles.color = Color.white;

        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.ViewRadius);
        Vector3 viewAngleA = fov.DirFromAngel(-fov.ViewAngle / 2);
        Vector3 viewAngleB = fov.DirFromAngel(fov.ViewAngle / 2);

        //Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        //Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        Handles.color = Color.red;

        foreach (var item in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, item.position);
        }
    }
}