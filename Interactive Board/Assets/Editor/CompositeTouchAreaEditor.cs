using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEditor.GenericMenu;

public enum EInsertType
{
    EMPTY,
    BOX,
    CAPSULE,
    CIRCLE,
    POLYGON,
}

[CustomEditor(typeof(CompositeTouchArea))]
public class CompositeTouchAreaEditor : Editor
{
    SerializedProperty touchAreasProp;

    private static int previewLinesPerUnit = 20;
    private static float previewPointTolerance = 0.001f;
    private static bool drawHorizontalPreviewLines = false;
    private static bool drawVerticalPreviewLines = false;

    private static bool showTouchAreas = false;
    private static List<bool> showIndividualTouchAreas = new List<bool>();

    private void OnEnable()
    {
        touchAreasProp = serializedObject.FindProperty("m_touchAreas");

        serializedObject.Update();
        if (touchAreasProp.arraySize == 0)
        {
            InsertTouchArea(-1, EInsertType.BOX);
        }
        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        CompositeTouchArea t = target as CompositeTouchArea;

        if (t == null || t.gameObject == null)
            return;

        float spacingUnit = 1.0f / previewLinesPerUnit;

        Bounds bounds = t.GetBounds();

        bounds.Expand(spacingUnit);

        if (drawHorizontalPreviewLines)
        {
            for (float y = bounds.min.y; y <= bounds.max.y; y += spacingUnit)
            {
                DrawPreviewLines(new Vector2(bounds.min.x, y), new Vector2(bounds.max.x, y), Vector2.right, Vector2.left, t, 0);
            }
        }

        if (drawVerticalPreviewLines)
        {
            for (float x = bounds.min.x; x <= bounds.max.x; x += spacingUnit)
            {
                DrawPreviewLines(new Vector2(x, bounds.min.y), new Vector2(x, bounds.max.y), Vector2.up, Vector2.down, t, 1);
            }
        }
    }

    private void DrawPreviewLines(Vector2 pointA, Vector2 pointB, Vector2 a2bUnit, Vector2 b2aUnit, CompositeTouchArea t, int index)
    {
        RaycastHit2D[] a2bHits = Physics2D.LinecastAll(pointA, pointB);
        RaycastHit2D[] b2aHits = Physics2D.LinecastAll(pointB, pointA);

        RaycastHit2D[] orderedHits = a2bHits.Concat(b2aHits).ToArray();

        if (orderedHits.Length == 0) return;

        Array.Sort(orderedHits, (a, b) => a.point[index] < b.point[index] ? -1 : a.point[index] > b.point[index] ? 1 : 0);

        for (int i = 0, j = 1; j < orderedHits.Length; i++, j++)
        {
            Vector2 aTolerance = orderedHits[i].point + a2bUnit * previewPointTolerance;
            Vector2 bTolerance = orderedHits[j].point + b2aUnit * previewPointTolerance;

            bool aOverlaps = t.OverlapPoint(aTolerance);
            bool bOverlaps = t.OverlapPoint(bTolerance);
            if (aOverlaps && bOverlaps)
            {
                Handles.color = Color.green;
                Handles.DrawLine(orderedHits[i].point, orderedHits[j].point);
            }
            else if (aOverlaps || bOverlaps)
            {
                Handles.color = Color.red;
                Handles.DrawLine(orderedHits[i].point, orderedHits[j].point);
                Handles.DrawSolidDisc(orderedHits[i].point, Vector3.forward, 0.01f);
                Handles.DrawSolidDisc(orderedHits[j].point, Vector3.forward, 0.01f);
            }
        }

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Event current = Event.current;

        TouchAreaGUI(current, 0);

        EditorGUILayout.Separator();

        Rect touchAreasHeader = EditorGUILayout.BeginHorizontal(GUILayout.Height(18));

        if (touchAreasHeader.Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();

            AddInsertTypeMenuItems(menu, touchAreasProp.arraySize - 1, "Add/");

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Reset"), false, CreateResetAllCallBack());

            menu.ShowAsContext();
        }

        if (touchAreasProp.arraySize != 1)
        {
            showTouchAreas = EditorGUILayout.PropertyField(touchAreasProp, false);
        }
        else
        {
            EditorGUILayout.LabelField("Touch Areas");
        }

        Rect dropDownRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(18));

        if (EditorGUILayout.DropdownButton(new GUIContent("Add"), FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();
            AddInsertTypeMenuItems(menu, touchAreasProp.arraySize - 1, "");
            menu.DropDown(dropDownRect);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        if (showTouchAreas && touchAreasProp.arraySize != 1)
        {
            EditorGUI.indentLevel = 1;

            for (int i = 1; i < touchAreasProp.arraySize; i++)
            {
                TouchAreaGUI(current, i);
            }

            EditorGUI.indentLevel = 0;
        }
        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Preview Lines Per Unit");
        previewLinesPerUnit = Mathf.Clamp(EditorGUILayout.IntField(previewLinesPerUnit), 0, 100);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Preview Point Tolerance");
        previewPointTolerance = Mathf.Clamp(EditorGUILayout.FloatField(previewPointTolerance), 0.00001f, 0.5f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Draw Horizontal Preview Lines");
        drawHorizontalPreviewLines = EditorGUILayout.Toggle(drawHorizontalPreviewLines);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Draw Vertical Preview Lines");
        drawVerticalPreviewLines = EditorGUILayout.Toggle(drawVerticalPreviewLines);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Refresh preview"))
        {
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void TouchAreaGUI(Event current, int i)
    {
        SerializedProperty touchArea = touchAreasProp.GetArrayElementAtIndex(i);
        string name = i == 0 ? "Base" : touchArea.FindPropertyRelative("area").objectReferenceValue?.name ?? $"Empty Area";

        GUI.SetNextControlName("area" + i);
        Rect touchAreaSection = EditorGUILayout.BeginHorizontal();

        if (touchAreaSection.Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            CreateTouchAreaContextMenu(current, i, name);
        }

        if (showIndividualTouchAreas.Count <= i)
        {
            showIndividualTouchAreas.Add(false);
        }

        showIndividualTouchAreas[i] = EditorGUILayout.Foldout(showIndividualTouchAreas[i], name, false);

        GUILayout.EndHorizontal();

        if (i < touchAreasProp.arraySize && showIndividualTouchAreas[i])
        {
            EditorGUILayout.PropertyField(touchArea.FindPropertyRelative("area"));

            if (i != 0)
            {
                EditorGUILayout.PropertyField(touchArea.FindPropertyRelative("compositeType"));
            }
        }
    }

    private void CreateTouchAreaContextMenu(Event current, int i, string name)
    {
        GenericMenu menu = new GenericMenu();

        AddInsertTypeMenuItems(menu, i, "Insert/");
        menu.AddSeparator("");

        menu.AddItem(new GUIContent($"Duplicate {name}"), false, CreateDuplicateTouchAreaCallBack(i));

        if (i > 0)
        {
            menu.AddItem(new GUIContent(i == 1 ? $"Swap {name} with Base" : "Move up"), false, CreateMoveTouchAreaCallBack(i, i - 1));
        }
        if (i < touchAreasProp.arraySize - 1)
        {
            menu.AddItem(new GUIContent("Move down"), false, CreateMoveTouchAreaCallBack(i, i + 1));
        }
        if (i != 0)
        {
            menu.AddItem(new GUIContent($"Delete {name}"), false, CreateDeleteTouchAreaCallBack(i));
        }

        menu.ShowAsContext();
    }

    private void AddInsertTypeMenuItems(GenericMenu menu, int i, string path)
    {
        menu.AddItem(new GUIContent($"{path}Empty Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.EMPTY));
        menu.AddSeparator(path);
        menu.AddItem(new GUIContent($"{path}Box Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.BOX));
        menu.AddItem(new GUIContent($"{path}Capsule Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.CAPSULE));
        menu.AddItem(new GUIContent($"{path}Circle Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.CIRCLE));
        menu.AddItem(new GUIContent($"{path}Polygon Touch Area"), false, CreateInsertTouchAreaCallBack(i, EInsertType.POLYGON));
    }

    private MenuFunction CreateInsertTouchAreaCallBack(int i, EInsertType type)
    {
        return () =>
        {
            serializedObject.Update();

            InsertTouchArea(i, type);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void InsertTouchArea(int i, EInsertType type, ECompositeType compositeType = ECompositeType.COMBINE)
    {
        switch (type)
        {
            case EInsertType.EMPTY:
                InsertTouchArea(i, null, compositeType);
                break;
            case EInsertType.BOX:
                InsertTouchArea<BoxCollider2D>(i, "Box Area", compositeType);
                break;
            case EInsertType.CAPSULE:
                InsertTouchArea<CapsuleCollider2D>(i, "Capsule Area", compositeType);
                break;
            case EInsertType.CIRCLE:
                InsertTouchArea<CircleCollider2D>(i, "Circle Area", compositeType);
                break;
            case EInsertType.POLYGON:
                InsertTouchArea<PolygonCollider2D>(i, "Polygon Area", compositeType);
                break;
        }
    }

    private void InsertTouchArea<T>(int i, string objectName, ECompositeType compositeType = ECompositeType.COMBINE) where T : Collider2D
    {
        GameObject gameObject = new GameObject(objectName, typeof(T));
        gameObject.transform.parent = (target as CompositeTouchArea).transform;
        gameObject.GetComponent<Collider2D>().isTrigger = true;

        InsertTouchArea(i, gameObject, compositeType);
    }

    private void InsertTouchArea(int i, GameObject gameObject, ECompositeType compositeTypeValue = ECompositeType.COMBINE)
    {
        touchAreasProp.InsertArrayElementAtIndex(i + 1);
        showIndividualTouchAreas.Insert(i + 1, false);

        SerializedProperty area = touchAreasProp.GetArrayElementAtIndex(i + 1).FindPropertyRelative("area");
        area.objectReferenceValue = gameObject;

        SerializedProperty compositeType = touchAreasProp.GetArrayElementAtIndex(i + 1).FindPropertyRelative("compositeType");
        compositeType.enumValueIndex = (int)compositeTypeValue;
    }

    private MenuFunction CreateDuplicateTouchAreaCallBack(int i)
    {
        return () =>
        {
            serializedObject.Update();

            DuplicateTouchArea(i);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void DuplicateTouchArea(int i)
    {
        Collider2D area = touchAreasProp.GetArrayElementAtIndex(i).FindPropertyRelative("area").objectReferenceValue as Collider2D;

        SerializedProperty compositeType = touchAreasProp.GetArrayElementAtIndex(i).FindPropertyRelative("compositeType");

        if (area == null)
        {
            InsertTouchArea(i, null, (ECompositeType)compositeType.enumValueIndex);
        }
        else
        {
            Type t = area.GetType();
            if (t == typeof(BoxCollider2D))
            {
                InsertTouchArea(i, EInsertType.BOX, (ECompositeType)compositeType.enumValueIndex);
            }
            else if (t == typeof(CapsuleCollider2D))
            {
                InsertTouchArea(i, EInsertType.CAPSULE, (ECompositeType)compositeType.enumValueIndex);
            }
            else if (t == typeof(CircleCollider2D))
            {
                InsertTouchArea(i, EInsertType.CIRCLE, (ECompositeType)compositeType.enumValueIndex);
            }
            else
            {
                InsertTouchArea(i, EInsertType.POLYGON, (ECompositeType)compositeType.enumValueIndex);
            }
        }
    }

    private MenuFunction CreateMoveTouchAreaCallBack(int src, int dest)
    {
        return () =>
        {
            serializedObject.Update();

            MoveTouchArea(src, dest);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void MoveTouchArea(int src, int dest)
    {
        touchAreasProp.MoveArrayElement(src, dest);

        bool temp = showIndividualTouchAreas[src];
        showIndividualTouchAreas[src] = showIndividualTouchAreas[dest];
        showIndividualTouchAreas[dest] = temp;
    }

    private MenuFunction CreateDeleteTouchAreaCallBack(int i)
    {
        return () =>
        {
            serializedObject.Update();

            DeleteTouchArea(i);

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void DeleteTouchArea(int i)
    {
        Collider2D area = touchAreasProp.GetArrayElementAtIndex(i).FindPropertyRelative("area").objectReferenceValue as Collider2D;
        if (area != null && area.transform.parent == (target as CompositeTouchArea).transform)
        {
            DestroyImmediate(area.gameObject);
        }

        touchAreasProp.DeleteArrayElementAtIndex(i);
        showIndividualTouchAreas.RemoveAt(i);
    }

    private MenuFunction CreateResetAllCallBack()
    {
        return () =>
        {
            serializedObject.Update();

            ResetAll();

            serializedObject.ApplyModifiedProperties();
        };
    }

    private void ResetAll()
    {
        while (touchAreasProp.arraySize > 0)
        {
            DeleteTouchArea(0);
        }
        InsertTouchArea(-1, EInsertType.BOX);

        previewLinesPerUnit = 20;
        previewPointTolerance = 0.001f;
        drawHorizontalPreviewLines = true;
        drawVerticalPreviewLines = true;
    }
}
