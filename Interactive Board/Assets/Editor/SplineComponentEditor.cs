using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.GenericMenu;

public enum eAxis
{
    X,
    Y,
    Z
}

[CustomEditor(typeof(SplineComponent))]
public class SplineComponentEditor : Editor
{
    int hotIndex = -1;
    int removeIndex = -1;

    const string SNAP_KEY = "SplineComponentEditor_Snap";
    const string SNAP_VALUE_KEY = "SplineComponentEditor_SnapValue";
    const string AXIS_KEY = "SplineComponentEditor_AXIS";


    bool snap = false;
    float snapValue = 1;
    eAxis axis = eAxis.Y;

    private void OnEnable()
    {
        snap = EditorPrefs.GetBool(SNAP_KEY, false);
        snapValue = EditorPrefs.GetFloat(SNAP_VALUE_KEY, 1.0f);
        axis = (eAxis)EditorPrefs.GetInt(AXIS_KEY, (int)eAxis.Y);
    }

    void OnSceneGUI()
    {
        var spline = target as SplineComponent;


        var e = Event.current;
        GUIUtility.GetControlID(FocusType.Passive);


        var mousePos = (Vector2)Event.current.mousePosition;
        var view = SceneView.currentDrawingSceneView.camera.ScreenToViewportPoint(Event.current.mousePosition);
        var mouseIsOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;
        if (mouseIsOutside) return;

        var points = serializedObject.FindProperty("points");
        if (Event.current.shift)
        {
            if (spline.closed)
                ShowClosestPointOnClosedSpline(points);
            else
                ShowClosestPointOnOpenSpline(points);
        }

        for (int i = 0; i < spline.points.Count; i++)
        {
            var prop = points.GetArrayElementAtIndex(i);
            var point = prop.vector3Value;
            var wp = spline.transform.TransformPoint(point);

            if (Event.current.type == EventType.MouseUp && snap)
            {
                Vector3 scaled = point / snapValue;
                point = new Vector3(Mathf.RoundToInt(scaled.x), Mathf.RoundToInt(scaled.y), Mathf.RoundToInt(scaled.z));
                point *= snapValue;
                prop.vector3Value = point;
                spline.ResetIndex();
            }


            if (hotIndex == i)
            {
                var newWp = Handles.PositionHandle(wp, Tools.pivotRotation == PivotRotation.Global ? Quaternion.identity : spline.transform.rotation);
                var delta = spline.transform.InverseTransformDirection(newWp - wp);
                if (delta.sqrMagnitude > 0)
                {
                    prop.vector3Value = point + delta;
                    spline.ResetIndex();
                }
                HandleCommands(wp);
            }

            Handles.color = i == 0 | i == spline.points.Count - 1 ? Color.red : Color.white;
            var buttonSize = HandleUtility.GetHandleSize(wp) * 0.1f;
            if (Handles.Button(wp, Quaternion.identity, buttonSize, buttonSize, Handles.SphereHandleCap))
                hotIndex = i;

            var v = SceneView.currentDrawingSceneView.camera.transform.InverseTransformPoint(wp);
            var labelIsOutside = v.z < 0;
            if (!labelIsOutside) Handles.Label(wp, i.ToString());
        }

        if (removeIndex >= 0 && points.arraySize > 4)
        {
            points.DeleteArrayElementAtIndex(removeIndex);
            spline.ResetIndex();
        }

        removeIndex = -1;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Hold Shift and click to append and insert curve points. Backspace to delete points.", MessageType.Info);
        var spline = target as SplineComponent;

        snap = EditorGUILayout.Toggle("Snap", snap);

        if (snap)
        {
            snapValue = EditorGUILayout.FloatField("Snap Value", snapValue);
        }

        EditorPrefs.SetBool(SNAP_KEY, snap);
        EditorPrefs.SetFloat(SNAP_VALUE_KEY, snapValue);

        var closed = EditorGUILayout.Toggle("Closed", spline.closed);
        if (spline.closed != closed)
        {
            spline.closed = closed;
            spline.ResetIndex();
        }

        EditorGUILayout.LabelField("Axis");
        if (EditorGUILayout.Toggle("X", axis == eAxis.X))
        {
            axis = eAxis.X;
        }
        if (EditorGUILayout.Toggle("Y", axis == eAxis.Y))
        {
            axis = eAxis.Y;
        }
        if (EditorGUILayout.Toggle("Z", axis == eAxis.Z))
        {
            axis = eAxis.Z;
        }
        EditorPrefs.SetInt(AXIS_KEY, (int)axis);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Center around Origin"))
        {
            Undo.RecordObject(target, "Center around Origin");
            CenterAroundOrigin(spline, spline.points);
            spline.ResetIndex();
        }

        Rect FlattenArea = EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button($"Flatten {axis} Axis"))
        {
            Undo.RecordObject(target, $"Flatten {axis} Axis");
            Vector3 projectionNormal = GetAxisNormal();
            Flatten(spline.points, projectionNormal, spline.transform.position);
            spline.ResetIndex();
        }


        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
    }

    private Vector3 GetAxisNormal()
    {
        switch (axis)
        {
            case eAxis.X:
                return Vector3.right;
            case eAxis.Y:
                return Vector3.up;
            case eAxis.Z:
                return Vector3.forward;
        }

        return Vector3.zero;
    }

    void HandleCommands(Vector3 wp)
    {
        if (Event.current.type == EventType.ExecuteCommand)
        {
            if (Event.current.commandName == "FrameSelected")
            {
                SceneView.currentDrawingSceneView.Frame(new Bounds(wp, Vector3.one * 10), false);
                Event.current.Use();
            }
        }
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Backspace)
            {
                removeIndex = hotIndex;
                Event.current.Use();
            }
        }
    }

    [DrawGizmo(GizmoType.NonSelected)]
    static void DrawGizmosLoRes(SplineComponent spline, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        DrawGizmo(spline, 64);
    }

    [DrawGizmo(GizmoType.Selected)]
    static void DrawGizmosHiRes(SplineComponent spline, GizmoType gizmoType)
    {
        Gizmos.color = Color.white;
        DrawGizmo(spline, 1024);
    }

    static void DrawGizmo(SplineComponent spline, int stepCount)
    {
        if (spline.points.Count > 0)
        {
            var P = 0f;
            var start = spline.GetNonUniformPoint(0);
            var step = 1f / stepCount;
            do
            {
                P += step;
                var here = spline.GetNonUniformPoint(P);
                Gizmos.DrawLine(start, here);
                start = here;
            } while (P + step <= 1);
        }
    }

    void ShowClosestPointOnClosedSpline(SerializedProperty points)
    {
        var spline = target as SplineComponent;
        var plane = new Plane(GetAxisNormal(), spline.transform.position);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float center;
        if (plane.Raycast(ray, out center))
        {
            var hit = ray.origin + ray.direction * center;
            Handles.DrawWireDisc(hit, GetAxisNormal(), 5);
            var p = SearchForClosestPoint(Event.current.mousePosition);
            var sp = spline.GetNonUniformPoint(p);
            Handles.DrawLine(hit, sp);


            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.shift)
            {
                var i = (Mathf.FloorToInt(p * spline.points.Count) + 2) % spline.points.Count;
                points.InsertArrayElementAtIndex(i);
                points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(sp);
                serializedObject.ApplyModifiedProperties();
                hotIndex = i;
            }
        }
    }


    void ShowClosestPointOnOpenSpline(SerializedProperty points)
    {
        var spline = target as SplineComponent;
        var plane = new Plane(GetAxisNormal(), spline.transform.position);
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float center;
        if (plane.Raycast(ray, out center))
        {
            var hit = ray.origin + ray.direction * center;
            var discSize = HandleUtility.GetHandleSize(hit);
            Handles.DrawWireDisc(hit, GetAxisNormal(), discSize);
            var p = SearchForClosestPoint(Event.current.mousePosition);


            //if ((hit - spline.GetNonUniformPoint(0)).sqrMagnitude < 25) p = 0;
            //if ((hit - spline.GetNonUniformPoint(1)).sqrMagnitude < 25) p = 1;


            var sp = spline.GetNonUniformPoint(p);


            var extend = Mathf.Approximately(p, 0) || Mathf.Approximately(p, 1);


            Handles.color = extend ? Color.red : Color.white;
            Handles.DrawLine(hit, sp);
            Handles.color = Color.white;


            var i = 1 + Mathf.FloorToInt(p * (spline.points.Count - 3));


            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.shift)
            {
                if (extend)
                {
                    if (i == spline.points.Count - 2) i++;
                    points.InsertArrayElementAtIndex(i);
                    points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(hit);
                    hotIndex = i;
                }
                else
                {
                    i++;
                    points.InsertArrayElementAtIndex(i);
                    points.GetArrayElementAtIndex(i).vector3Value = spline.transform.InverseTransformPoint(sp);
                    hotIndex = i;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }


    float SearchForClosestPoint(Vector2 screenPoint, float A = 0f, float B = 1f, float steps = 1000)
    {
        var spline = target as SplineComponent;
        var smallestDelta = float.MaxValue;
        var step = (B - A) / steps;
        var closestI = A;
        for (var i = 0; i <= steps; i++)
        {
            var p = spline.GetNonUniformPoint(i * step);
            var gp = HandleUtility.WorldToGUIPoint(p);
            var delta = (screenPoint - gp).sqrMagnitude;
            if (delta < smallestDelta)
            {
                closestI = i;
                smallestDelta = delta;
            }
        }
        return closestI * step;
    }

    MenuFunction MakeFlattenDelegate(SplineComponent spline, string axisName, Vector3 projectionNormal)
    {
        return () =>
        {

        };
    }

    void Flatten(List<Vector3> points, Vector3 projectionNormal, Vector3 center)
    {
        for (int i = 0; i < points.Count; i++)
        {
            points[i] = points[i] - Vector3.Project(points[i] - center, projectionNormal);
        }
    }


    void CenterAroundOrigin(SplineComponent spline, List<Vector3> points)
    {
        var center = Vector3.zero;
        for (int i = 0; i < points.Count; i++)
        {
            center += points[i];
        }
        center /= points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= center;
        }
    }
}